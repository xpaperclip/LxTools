using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LxTools.Liquipedia
{
    public class LpTemplateWriter
    {
        public LpTemplateWriter(string template)
        {
            this.template = template;
        }
        public static LpTemplateWriter FromFile(string filename)
        {
            return new LpTemplateWriter(File.ReadAllText(filename));
        }

        private string template;
        public string Template
        {
            get { return this.template; }
        }

        public string Write(Bag properties)
        {
            string text = template;
            foreach (var kvp in properties)
            {
                string param = string.Format("|{0}=", kvp.Key);
                text = text.Replace(param, param + kvp.Value);
            }
            return text;
        }
    }
}
