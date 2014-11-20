using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace LxTools.Liquipedia
{
    public static class LiquipediaClient
    {
        public static bool IsValidLiquipediaLink(string page)
        {
            // edit link
            if (page.StartsWith("http://wiki.teamliquid.net/starcraft2/index.php?title=") && page.Contains("action=edit"))
                return true;
            if (page.StartsWith("http://wiki.teamliquid.net/starcraft/index.php?title=") && page.Contains("action=edit"))
                return true;

            // otherwise just a direct page link
            Uri uri;
            if (Uri.TryCreate(page, UriKind.Absolute, out uri) && 
                uri.Host.ToLower() == "wiki.teamliquid.net" &&
                (uri.AbsolutePath.StartsWith("/starcraft2/") || uri.AbsolutePath.StartsWith("/starcraft/")))
            {
                return true;
            }

            // otherwise we don't know how to get the page data
            return false;
        }
        public static string GetWikicode(string page)
        {
            // edit link
            if ((page.StartsWith("http://wiki.teamliquid.net/starcraft2/index.php?title=") && page.Contains("action=edit")) ||
                (page.StartsWith("http://wiki.teamliquid.net/starcraft/index.php?title=") && page.Contains("action=edit")))
            {
                using (WebClient wc = new WebClient())
                {
                    string text = wc.DownloadString(page);
                    text = text.TrimBetween("<textarea", "</textarea>").From(">").Replace("&lt;", "<").Replace("&amp;", "&");
                    return text;
                }
            }

            // if it's a link, extract the page title
            Uri uri;
            if (Uri.TryCreate(page, UriKind.Absolute, out uri) &&
                uri.Host.ToLower() == "wiki.teamliquid.net" && 
                uri.AbsolutePath.StartsWith("/starcraft2/"))
            {
                page = uri.AbsolutePath.Substring("/starcraft2/".Length);
                page = Uri.UnescapeDataString(page);
            }

            // get page data
            string url = "http://wiki.teamliquid.net/starcraft2/api.php?format=xml&action=query&titles=" +
                Uri.EscapeUriString(page.Replace("%20", " ").Replace(" ", "_")) + "&prop=revisions&rvprop=content";

            // if it's a bw link, extract the page title and get the data
            if (Uri.TryCreate(page, UriKind.Absolute, out uri) &&
                uri.Host.ToLower() == "wiki.teamliquid.net" &&
                uri.AbsolutePath.StartsWith("/starcraft/"))
            {
                page = Uri.EscapeUriString(uri.AbsolutePath.Substring("/starcraft/".Length));
                page = Uri.UnescapeDataString(page);
                url = "http://wiki.teamliquid.net/starcraft/api.php?format=xml&action=query&titles=" +
                    Uri.EscapeUriString(page.Replace("%20", " ").Replace(" ", "_")) + "&prop=revisions&rvprop=content";
            }
            
            XDocument xml = XDocument.Load(url);
            XElement xpage = xml.Elements("api").Elements("query").Elements("pages").Elements("page").First();
            string data = xpage.Elements("revisions").Elements("rev").First().Value;
            return data;
        }
    }
}
