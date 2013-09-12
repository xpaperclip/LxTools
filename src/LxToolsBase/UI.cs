using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LxTools
{
    public static class UI
    {
        public static DialogResult InputBox(string title, string prompt, ref string value)
        {
            using (var form = new Form())
            using (var label = new Label())
            using (var textBox = new TextBox())
            using (var buttonOk = new Button())
            using (var buttonCancel = new Button())
            {
                form.Text = title;
                label.Text = prompt;
                textBox.Text = value;

                form.Font = new Font("Tahoma", 8);

                buttonOk.Text = "OK";
                buttonCancel.Text = "Cancel";
                buttonOk.DialogResult = DialogResult.OK;
                buttonCancel.DialogResult = DialogResult.Cancel;

                label.SetBounds(9, 20, 372, 13);
                textBox.SetBounds(12, 36, 372, 20);
                buttonOk.SetBounds(228, 72, 75, 23);
                buttonCancel.SetBounds(309, 72, 75, 23);

                label.AutoSize = true;
                textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                form.ClientSize = new Size(396, 107);
                form.Controls.AddRange(new Control[] { textBox, label, buttonOk, buttonCancel });
                form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                DialogResult result = form.ShowDialog();
                value = textBox.Text;
                return result;
            }
        }

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
                frm.Text = "LxTools";
                frm.Controls.Add(tabs);
                frm.Size = new Size(700, 500);
                frm.ShowDialog();
            }
        }
    }

    public struct UIDocument
    {
        public UIDocument(string title, string text)
            : this()
        {
            this.Title = title;
            this.Text = text;
        }

        public string Title { get; set; }
        public string Text { get; set; }
    }
}
