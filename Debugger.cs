using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebAppKit
{
    public static class Debugger
    {
        public static Form Source = new Form();
        public static TextBox tb = new TextBox();
        public static void Show()
        {
            Source.Show();
            Source.Controls.Add(tb);
            tb.Multiline = true;
            tb.ScrollBars = ScrollBars.Vertical;
            tb.Dock = DockStyle.Fill;
            tb.ReadOnly = true;
        }
        public static void Update(string data)
        {
            tb.Text = data;
        }
    }
}
