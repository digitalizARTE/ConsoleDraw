using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleDraw
{
    public class Cursor
    {
        public bool _cursorShow;
        public int _x;
        public int _y;
        public Timer blink;
        public char blinkLetter;
        public ConsoleColor _background;
        private bool visible;

        public void PlaceCursor(int x, int y, char letter, ConsoleColor background = ConsoleColor.Blue)
        {
            this.visible = true;
            this._x = x;
            this._y = y;
            this.blinkLetter = letter == '\r' || letter == '\n' ? ' ' : letter;
            this._background = background;
            WindowManager.WirteText("_", x, y, ConsoleColor.White, background);

            this.blink = new Timer(500);
            this.blink.Elapsed += new ElapsedEventHandler(this.BlinkCursor);
            this.blink.Enabled = true;
        }

        public void RemoveCursor()
        {
            if (this.visible)
            {
                WindowManager.WirteText(" ", this._x, this._y, ConsoleColor.White, this._background);
                if (this.blink != null) this.blink.Dispose();
                this.visible = false;
            }
            
        }

        void BlinkCursor(object sender, ElapsedEventArgs e)
        {
            if (this._cursorShow)
            {
                WindowManager.WirteText(this.blinkLetter.ToString(), this._x, this._y, ConsoleColor.White, this._background);
                this._cursorShow = false;
            }
            else
            {
                WindowManager.WirteText("_", this._x, this._y, ConsoleColor.White, this._background);
                this._cursorShow = true;
            }
        }


    }
}
