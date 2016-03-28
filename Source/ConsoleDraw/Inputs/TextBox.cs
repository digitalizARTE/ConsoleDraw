using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class TextBox : Input
    {
        private bool Selected = false;

        private int cursorPostion;
        private int CursorPostion { get { return this.cursorPostion; } set {
            this.cursorPostion = value;
            this.SetOffset(); } }

        private int Offset = 0;
        private string Text = string.Empty;

        private ConsoleColor TextColour = ConsoleColor.White;
        private ConsoleColor BackgroundColour = ConsoleColor.DarkGray;

        private readonly Cursor cursor = new Cursor();

        public TextBox(int x, int y, string iD, Window parentWindow, int length = 38) : base(x, y, 1, length, parentWindow, iD)
        {
            this.Selectable = true;
        }

        public TextBox(int x, int y, string text, string iD, Window parentWindow, int length = 38) : base(x, y, 1, length, parentWindow, iD)
        {
            this.Text = text;

            this.CursorPostion = text.Length;

            this.Selectable = true;
        }

        public override void Select()
        {
            if (!this.Selected)
            {
                this.Selected = true;
                this.Draw();
            }
        }

        public override void Unselect()
        {
            if (this.Selected)
            {
                this.Selected = false;
                this.Draw();
            }
        }

        public override void Enter()
        {
            this.ParentWindow.MoveToNextItem();
        }

        public override void AddLetter(char letter)
        {
            string textBefore = this.Text.Substring(0, this.CursorPostion);
            string textAfter = this.Text.Substring(this.CursorPostion, this.Text.Length - this.CursorPostion);

            this.Text = textBefore + letter + textAfter;
            this.CursorPostion++;
            this.Draw();
        }

        public override void BackSpace()
        {
            if (this.CursorPostion != 0)
            {
                string textBefore = this.Text.Substring(0, this.CursorPostion);
                string textAfter = this.Text.Substring(this.CursorPostion, this.Text.Length - this.CursorPostion);

                textBefore = textBefore.Substring(0, textBefore.Length - 1);

                this.Text = textBefore + textAfter;
                this.CursorPostion--;
                this.Draw();
            }
        }

        public override void CursorMoveLeft()
        {
            if (this.CursorPostion != 0)
            {
                this.CursorPostion--;
                this.Draw();
            }
            else this.ParentWindow.MovetoNextItemLeft(this.Xpostion - 1, this.Ypostion, 3);
        }

        public override void CursorMoveRight()
        {
            if (this.CursorPostion != this.Text.Length)
            {
                this.CursorPostion++;
                this.Draw();
            }
            else this.ParentWindow.MovetoNextItemRight(this.Xpostion - 1, this.Ypostion + this.Width, 3);
        }

        public override void CursorToStart()
        {
            this.CursorPostion = 0;
            this.Draw();
        }

        public override void CursorToEnd()
        {
            this.CursorPostion = this.Text.Length;
            this.Draw();
        }

        public string GetText()
        {
            return this.Text;
        }

        public void SetText(string text)
        {
            this.Text = text;
            this.Draw();
        }

        public override void Draw()
        {
            this.RemoveCursor();

            string clippedPath = "";

            if (this.Selected)
            {
                clippedPath = string.Format(
                    "{0}{1}",
                    ' ',
                    this.Text.PadRight(this.Width + this.Offset, ' ').Substring(this.Offset, this.Width - 2));
            }
            else
            {
                clippedPath = string.Format("{0}{1}", ' ', this.Text.PadRight(this.Width, ' ').Substring(0, this.Width - 2));
            }

            WindowManager.WirteText(string.Format("{0} ", clippedPath), this.Xpostion, this.Ypostion, this.TextColour, this.BackgroundColour);
            if (this.Selected)
            {
                this.ShowCursor();
            }          
        }

        private void ShowCursor()
        {
            string paddedText = string.Format("{0} ", this.Text);
            this.cursor.PlaceCursor(this.Xpostion, this.Ypostion + this.CursorPostion - this.Offset + 1, paddedText[this.CursorPostion], this.BackgroundColour);
        }

        private void RemoveCursor()
        {
            this.cursor.RemoveCursor();
        }

        private void SetOffset()
        {
            while (this.CursorPostion - this.Offset > this.Width - 2)
            {
                this.Offset++;
            }

            while (this.CursorPostion - this.Offset < 0)
            {
                this.Offset--;
            }
        }



        public override void CursorMoveDown()
        {
            this.ParentWindow.MovetoNextItemDown(this.Xpostion, this.Ypostion, this.Width);
        }

        public override void CursorMoveUp()
        {
            this.ParentWindow.MovetoNextItemUp(this.Xpostion, this.Ypostion, this.Width);
        }
    }
}
