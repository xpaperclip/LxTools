using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace LxTools.Tlpd
{
    public static class TlpdClient
    {
        public static HttpWebRequest GetRequest(string request, string mode)
        {
            var encoded = System.Uri.EscapeDataString(request);
            var requestString = string.Format("text={0}&mode=tlpd-{1}", encoded, mode);

            System.Net.ServicePointManager.Expect100Continue = false;
            var webRequest = (HttpWebRequest)WebRequest.Create("http://www.teamliquid.net/tlpd/tlpdize.php");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = requestString.Length;
            webRequest.Accept = "*/*";
            webRequest.Timeout = 10000;
            using (var stream = webRequest.GetRequestStream())
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(requestString);
            }

            return webRequest;
        }
        public static string Request(string request, string mode)
        {
            var webRequest = GetRequest(request, mode);
            var response = webRequest.GetResponse();
            
            using (var stream = response.GetResponseStream())
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public static IEnumerable<TlpdEntity> ParseMany(string request, string mode)
        {
            string s = Request(request, mode);

            int upto;
            while (s.Length > 0)
            {
                var entity = Parse(s, false, out upto);
                if (!entity.IsValid)
                {
                    yield break;
                }

                yield return entity;
                s = s.Substring(upto);
                if (s.Length == 0)
                    yield break;

                if (s.StartsWith(" OR "))
                    s = s.Substring(4);
            }
        }

        public static TlpdEntity Parse(string tlpd)
        {
            return Parse(tlpd, true);
        }
        public static TlpdEntity Parse(string tlpd, bool allowJustOpen)
        {
            int length;
            return Parse(tlpd, allowJustOpen, out length);
        }
        public static TlpdEntity Parse(string tlpd, bool allowJustOpen, out int length)
        {
            if (!tlpd.StartsWith("[tlpd#"))
            {
                // reject it immediately
                length = 0;
                return TlpdEntity.InvalidEntity;
            }

            // find end of tlpd string
            int closebracket = tlpd.IndexOf("]");
            if (closebracket < 0)
            {
                // incomplete tag, reject
                length = 0;
                return TlpdEntity.InvalidEntity;
            }

            // find closing tag if required
            TlpdEntity entity = new TlpdEntity();
            int closingtag = tlpd.IndexOf("[/tlpd]");
            if (closingtag < 0)
            {
                length = closebracket + 1;
                if (!allowJustOpen)
                {
                    // incomplete tag, reject it now
                    return TlpdEntity.InvalidEntity;
                }
            }
            else
            {
                length = closingtag + "[/tlpd]".Length;
                if (closebracket > closingtag)
                {
                    length = closebracket;
                    return TlpdEntity.InvalidEntity;
                }
                entity.Name = tlpd.Substring(closebracket + 1, closingtag - (closebracket + 1));
            }

            // get tag bits
            string param = tlpd.Substring(0, closebracket);
            param = param.Substring("[tlpd#".Length);
            string[] tags = param.Split('#');
            if (tags.Length < 2)
            {
                // not enough arguments, reject it
                return TlpdEntity.InvalidEntity;
            }

            entity.Id = tags[1];
            switch (tags[0])
            {
                case "maps":
                    entity.Type = TlpdEntityType.Map;
                    if (tags.Length > 2) entity.Database = tags[2];
                    if (tags.Length > 3) entity.Race = tags[3];
                    break;
                case "players":
                    // players must have a race
                    if (tags.Length < 3) return TlpdEntity.InvalidEntity;
                    entity.Type = TlpdEntityType.Player;
                    entity.Race = tags[2];
                    if (tags.Length > 3) entity.Database = tags[3];
                    break;
                default:
                    return TlpdEntity.InvalidEntity;
            }

            // [tlpd#maps#id#db#tileset]name[/tlpd]
            // [tlpd#players#id#R#db]name[/tlpd]

            return entity;
        }
    }
}
