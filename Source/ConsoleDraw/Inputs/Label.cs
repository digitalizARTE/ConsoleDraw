using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class Label : Input
    {
        private string Text = string.Empty;
        private ConsoleColor TextColour = ConsoleColor.Black;
        public ConsoleColor BackgroundColour = ConsoleColor.Gray;

        public Label(string text, int x, int y, string iD, Window parentWindow) : base(x, y, 1, text.Count(), parentWindow, iD)
        {
            this.Text = text;
            this.BackgroundColour = parentWindow.BackgroundColour;
            this.Selectable = false;
        }

        public override void Draw()
        {
            WindowManager.WirteText(this.Text, this.Xpostion, this.Ypostion, this.TextColour, this.BackgroundColour);
        }

        public void SetText(string text)
        {
            this.Text = text;
            this.Width = text.Count();
            this.Draw();
        }
       
    }
}
