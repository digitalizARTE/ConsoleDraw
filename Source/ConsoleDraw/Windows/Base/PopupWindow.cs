using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Windows.Base
{
    public class PopupWindow : Window
    {
        protected string Title;

        protected ConsoleColor TitleBarColour = ConsoleColor.DarkGray;
        protected ConsoleColor TitleColour = ConsoleColor.Black;

        public PopupWindow(string title, int postionX, int postionY, int width, int height, Window parentWindow)
            : base(postionX, postionY, width, height, parentWindow)
        {
            this.Title = title;
        }

        public override void ReDraw()
        {
            WindowManager.DrawColourBlock(this.TitleBarColour, this.PostionX, this.PostionY, this.PostionX + 1, this.PostionY + this.Width); //Title Bar
            WindowManager.WirteText(string.Format("{0}{1}{0}", ' ', this.Title), this.PostionX, this.PostionY + 2, this.TitleColour, this.BackgroundColour);
            WindowManager.DrawColourBlock(this.BackgroundColour, this.PostionX + 1, this.PostionY, this.PostionX + this.Height, this.PostionY + this.Width); //Main Box
        }

    }
}
