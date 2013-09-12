using System;
using System.Linq;
using System.Text;

namespace LxTools
{
    public static class StringUtils
    {
        public static string TrimWhitespace(this string s)
        {
            if (s == null) return null;
            return s.Trim("\r\n\t ".ToCharArray());
        }

        public static string UpTo(this string s, string needle)
        {
            int idx = s.IndexOf(needle);
            if (idx < 0) return s;
            return s.Substring(0, idx);
        }
        public static string From(this string s, string needle)
        {
            int idx = s.IndexOf(needle);
            if (idx < 0) return "";
            return s.Substring(idx + needle.Length);
        }
        public static Tuple<string, string> SplitAt(this string s, string needle)
        {
            int idx = s.IndexOf(needle);
            if (idx < 0) return new Tuple<string, string>(s, "");
            return new Tuple<string, string>(s.Substring(0, idx), s.Substring(idx + needle.Length));
        }
        public static string SplitAt(this string s, string needle, out string first)
        {
            int idx = s.IndexOf(needle);
            if (idx < 0)
            {
                first = s;
                return "";
            }
            else
            {
                first = s.Substring(0, idx);
                return s.Substring(idx + needle.Length);
            }
        }

        public static string TrimBefore(this string s, string a)
        {
            int bra = s.IndexOf(a);
            return s.Substring(0, bra).Trim();
        }
        public static string TrimAfter(this string s, string a)
        {
            int bra = s.IndexOf(a);
            return s.Substring(bra + a.Length).Trim();
        }
        public static string TrimBetween(this string s, string a, string b)
        {
            int bra = s.IndexOf(a);
            int ket = s.IndexOf(b, bra + a.Length);
            return s.Substring(bra + a.Length, ket - bra - a.Length).Trim();
        }

        public static string Trim(this string s, out int start, out int end)
        {
            end = s.Length - 1;
            start = 0;
            while (start < s.Length)
            {
                if (!char.IsWhiteSpace(s[start]))
                    break;
                start++;
            }
            while (end >= start)
            {
                if (!char.IsWhiteSpace(s[end]))
                    break;
                end--;
            }
            return s.Substring(start, end - start + 1);
        }
        public static string TrimLeft(this string s, out int start)
        {
            start = 0;
            while (start < s.Length)
            {
                if (!char.IsWhiteSpace(s[start]))
                    break;
                start++;
            }
            return s.Substring(start);
        }

        public static string MaxSubstring(this string s, int length)
        {
            return s.Substring(0, s.Length > length ? length : s.Length);
        }

        public static string StripBBCode(string s)
        {
            var sb = new StringBuilder();
            int upto = 0;
            while (true)
            {
                int idx = s.IndexOf("[", upto);
                if (idx < 0) break;

                // stuff before it
                sb.Append(s.Substring(upto, idx - upto));

                idx = s.IndexOf("]", idx);
                upto = idx + 1;
            }
            sb.Append(s.Substring(upto));
            return sb.ToString();
        }
        public static string StripHtmlComments(string s)
        {
            var sb = new StringBuilder();
            int upto = 0;
            while (true)
            {
                int idx = s.IndexOf("<!--", upto);
                if (idx < 0) break;

                // stuff before it
                sb.Append(s.Substring(upto, idx - upto));

                idx = s.IndexOf("-->", idx);
                upto = idx + 3;
            }
            sb.Append(s.Substring(upto));
            return sb.ToString();
        }

        public static string Repeat(this string str, int count)
        {
            return string.Concat(Enumerable.Repeat(str, count));
        }
    }
}
