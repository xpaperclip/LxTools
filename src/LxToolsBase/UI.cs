using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LxTools
{
    public class UIDocument
    {
        public UIDocument(string title, string text)
        {
            this.Title = title;
            this.Text = text;
        }

        public string Title { get; set; }
        public string Text { get; set; }
    }

    public static class UI
    {
        public static void ShowDialog(params UIDocument[] docs)
        {
            Application.EnableVisualStyles();

            using (var frm = new Form())
            using (var tabs = new TabControl())
            {
                tabs.Dock = DockStyle.Fill;
                foreach (UIDocument doc in docs)
                {
                    var txt = new RichTextBox();
                    txt.Font = new Font("Consolas", 9);
                    txt.Dock = DockStyle.Fill;
                    txt.Multiline = true;
                    txt.ScrollBars = RichTextBoxScrollBars.Both;
                    txt.Text = doc.Text;
                    txt.ShortcutsEnabled = true;

                    var page = new TabPage();
                    page.Text = doc.Title;
                    page.Controls.Add(txt);
                    tabs.TabPages.Add(page);
                }

                frm.Font = new Font("Segoe UI", 9);
                frm.Text = "LpTools";
                frm.Controls.Add(tabs);
                frm.Size = new Size(700, 500);
                frm.ShowDialog();
            }
        }
    }
}
