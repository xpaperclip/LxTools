using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LxTools.Liquipedia.Parsing
{
    public class WikiParser
    {
        public static List<IWikiItem> Parse(string s, int start, out int idx)
        {
            return ParseTempl(s, start, out idx, true).Children;
        }
        public static WikiComment ParseComment(string s, int start, out int idx)
        {
            idx = start;
            int closingidx = s.IndexOf("-->", idx);
            if (closingidx < 0)
            {
                WikiComment comment = new WikiComment(s.Substring(idx));
                idx = s.Length;
                return comment;
            }
            else
            {
                WikiComment comment = new WikiComment(s.Substring(idx, closingidx - idx));
                idx = closingidx + 3;
                return comment;
            }
        }
        public static WikiTemplate ParseTempl(string s, int start, out int idx, bool rootlevel)
        {
            idx = start;
            WikiTemplate template = new WikiTemplate();
            List<IWikiItem> items = template.Children;
            while (idx < s.Length)
            {
                int commentopenindex = s.IndexOf("<!--", idx);
                int templatecloseindex = rootlevel ? -1 : s.IndexOf("}}", idx);
                int templateopenindex = s.IndexOf("{{", idx);
                if ((commentopenindex >= 0) && ((templatecloseindex < 0) || (commentopenindex < templatecloseindex)) && ((templateopenindex < 0) || (commentopenindex < templateopenindex)))
                {
                    string text = s.Substring(idx, commentopenindex - idx).TrimWhitespace();
                    if (!string.IsNullOrEmpty(text))
                        items.Add(new WikiText(text));
                    items.Add(ParseComment(s, commentopenindex + 4, out idx));
                }
                else if ((templateopenindex < 0) || ((templatecloseindex >= 0) && (templatecloseindex < templateopenindex)))
                {
                    if (templatecloseindex < 0)
                    {
                        string text = s.Substring(idx).TrimWhitespace();
                        if (!string.IsNullOrEmpty(text))
                            items.Add(new WikiText(text));
                        idx = s.Length;
                    }
                    else
                    {
                        string text = s.Substring(idx, templatecloseindex - idx).TrimWhitespace();
                        if (!string.IsNullOrEmpty(text))
                            items.Add(new WikiText(text));
                        idx = templatecloseindex + 2;
                        break;
                    }
                }
                else if (templateopenindex == idx)
                {
                    items.Add(ParseTempl(s, templateopenindex + 2, out idx, false));
                }
                else
                {
                    string text = s.Substring(idx, templateopenindex - idx);
                    if (!string.IsNullOrEmpty(text))
                        items.Add(new WikiText(text));
                    items.Add(ParseTempl(s, templateopenindex + 2, out idx, false));
                }
            }

            // now process into params
            if (!rootlevel)
            {
                List<IWikiItem> currentParam = new List<IWikiItem>();
                int unnamed = 0;
                string paramName = "0";
                Queue<IWikiItem> remainingItems = new Queue<IWikiItem>(template.Children);
                while (remainingItems.Count > 0)
                {
                    IWikiItem item = remainingItems.Dequeue();
                    if (item is WikiText)
                    {
                        string text = (item as WikiText).Text;
                        int baridx;
                        while ((baridx = text.IndexOf('|')) >= 0)
                        {
                            // add up to the |
                            string tt = text.Substring(0, baridx).TrimWhitespace();
                            if (!string.IsNullOrEmpty(tt))
                                currentParam.Add(new WikiText(tt));
                            template.Params[paramName] = currentParam;
                            currentParam = new List<IWikiItem>();

                            text = text.Substring(baridx + 1);
                            int nextbar = text.IndexOf('|');
                            int equalsidx;
                            if (nextbar > 0)
                                equalsidx = text.IndexOf('=', 0, nextbar);
                            else
                                equalsidx = text.IndexOf('=');
                            if (equalsidx < 0)
                            {
                                unnamed++;
                                paramName = unnamed.ToString();
                            }
                            else
                            {
                                paramName = text.Substring(0, equalsidx).TrimWhitespace();
                                text = text.Substring(equalsidx + 1);
                            }
                        }
                        text = text.TrimWhitespace();
                        if (text.Length > 0) currentParam.Add(new WikiText(text));
                    }
                    else
                    {
                        currentParam.Add(item);
                    }
                }
                if (currentParam.Count != 0)
                {
                    template.Params[paramName] = currentParam;
                }
            }
            return template;
        }
    }

    public interface IWikiItem { }
    public class WikiText : IWikiItem
    {
        public WikiText(string Text)
        {
            this.Text = Text;
        }
        public string Text;

        public override string ToString()
        {
            return this.Text;
        }
    }
    public class WikiComment : IWikiItem
    {
        public WikiComment(string Text)
        {
            this.Text = Text;
        }
        public string Text;

        public override string ToString()
        {
            return this.Text;
        }
    }
    public class WikiTemplate : IWikiItem
    {
        public readonly List<IWikiItem> Children = new List<IWikiItem>();
        public readonly Dictionary<string, List<IWikiItem>> Params = new Dictionary<string, List<IWikiItem>>();

        public string Name
        {
            get
            {
                if (Params.ContainsKey("0"))
                {
                    var p = Params["0"];
                    if ((p.Count > 0) && (p[0] is WikiText))
                        return (p[0] as WikiText).Text;
                }
                return "";
            }
        }

        public string this[string label]
        {
            get { return GetParam(label); }
        }

        public string GetParam(string label)
        {
            if (label == null || !Params.ContainsKey(label)) return null;
            return (from item in Params[label]
                    where item is WikiText || item is WikiTemplate
                    select item.ToString()).FirstOrDefault();
        }
        public string GetParamText(string label)
        {
            if (label == null || !Params.ContainsKey(label)) return null;
            return (from item in Params[label]
                    where item is WikiText
                    select (item as WikiText).Text).FirstOrDefault();
        }
        public WikiTemplate GetParamTemplate(string label, string template)
        {
            if (label == null || !Params.ContainsKey(label)) return null;
            return (from item in Params[label]
                    where item is WikiTemplate
                    let templ = item as WikiTemplate
                    where templ.Name == template
                    select templ).First();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}