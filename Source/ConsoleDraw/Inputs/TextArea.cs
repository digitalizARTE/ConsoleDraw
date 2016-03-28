using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class TextArea : Input
    {
        private bool Selected = false;

        private int CursorPostion;

        private int cursorDisplayX;
        private int CursorDisplayX { get { return this.cursorDisplayX; } set {
            this.cursorDisplayX = value;
            this.SetOffset(); } }

        private int CursorDisplayY;

        private int Offset = 0;
        private List<string> SplitText = new List<string>();
        private string text = "";
        private string Text {
            get{
                return this.text;
            } 
            set {
                if (this.OnChange != null && this.text != value) this.OnChange();

                this.text = value;

                this.SplitText = this.CreateSplitText();
            }
        }
        private string TextWithoutNewLine { get { return this.RemoveNewLine(this.Text); } }

        private ConsoleColor TextColour = ConsoleColor.White;
        public ConsoleColor BackgroundColour = ConsoleColor.Blue;

        private Cursor cursor = new Cursor();

        public Action OnChange;

        public TextArea(int x, int y, int width, int height, string iD, Window parentWindow) : base(x, y, height, width, parentWindow, iD)
        {
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

        public override void AddLetter(char letter)
        {
            string textBefore = this.Text.Substring(0, this.CursorPostion);
            string textAfter = this.Text.Substring(this.CursorPostion, this.Text.Length - this.CursorPostion);

            this.Text = textBefore + letter + textAfter;

            this.CursorPostion++;
            this.Draw();
        }

        public override void CursorMoveLeft()
        {
            if (this.CursorPostion != 0) this.CursorPostion--;

            this.Draw();
        }

        public override void CursorMoveRight()
        {
            if (this.CursorPostion != this.Text.Length)
            {
                this.CursorPostion++;
                this.Draw();
            }
        }

        public override void CursorMoveDown()
        {
            List<string> splitText = this.SplitText;

            if (splitText.Count == this.CursorDisplayX + 1) //Cursor at end of text in text area
            {
                this.ParentWindow.MovetoNextItemDown(this.Xpostion, this.Ypostion, this.Width);
                return;
            }

            string nextLine = splitText[this.CursorDisplayX + 1];

            int newCursor = 0;
            for (int i = 0; i < this.cursorDisplayX + 1; i++)
            {
                newCursor += splitText[i].Count();
            }

            if (nextLine.Count() > this.CursorDisplayY)
            {
                newCursor += this.CursorDisplayY;
            }
            else
            {
                newCursor += nextLine.Count(x => x != '\n');
            }

            this.CursorPostion = newCursor;

            this.Draw();
        }

        public override void CursorMoveUp()
        {
            List<string> splitText = this.SplitText;

            if (0 == this.CursorDisplayX) //Cursor at top of text area
            {
                this.ParentWindow.MovetoNextItemUp(this.Xpostion, this.Ypostion, this.Width);
                return;
            }

            string nextLine = splitText[this.CursorDisplayX - 1];

            int newCursor = 0;
            for (int i = 0; i < this.cursorDisplayX - 1; i++)
            {
                newCursor += splitText[i].Count();
            }

            if (nextLine.Count() >= this.CursorDisplayY)
                newCursor += this.CursorDisplayY;
            else
                newCursor += nextLine.Count(x => x!='\n');

            this.CursorPostion = newCursor;
            this.Draw();
        }

        public override void CursorToStart()
        {
            List<string> splitText = this.SplitText;

            int newCursor = 0;
            for (int i = 0; i < this.cursorDisplayX; i++)
            {
                newCursor += splitText[i].Count();
            }

            this.CursorPostion = newCursor;
            this.Draw();
        }

        public override void CursorToEnd()
        {
            List<string> splitText = this.SplitText;
            string currentLine = splitText[this.cursorDisplayX];

            int newCursor = 0;
            for (int i = 0; i < this.cursorDisplayX + 1; i++)
            {
                newCursor += splitText[i].Count();
            }

            this.CursorPostion = newCursor - currentLine.Count(x => x == '\n');
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

        public override void Enter()
        {
            this.AddLetter('\n');
        }

        public void SetText(string text)
        {
            this.Text = text;
            this.CursorPostion = 0;
            this.Draw();
        }

        public string GetText()
        {
            return this.Text;
        }

        public override void Draw()
        {
            this.RemoveCursor();

            this.UpdateCursorDisplayPostion();

            List<string> lines = this.SplitText;

            //Draw test area
            for (int i = this.Offset; i < this.Height + this.Offset; i++)
            {
                string line = string.Format("{0}{1}", ' ', string.Empty.PadRight(this.Width - 1, ' '));
                if (lines.Count > i)
                {
                    line = ' ' + this.RemoveNewLine(lines[i]).PadRight(this.Width - 1, ' ');
                }

                WindowManager.WirteText(line, i + this.Xpostion - this.Offset, this.Ypostion, this.TextColour, this.BackgroundColour);
            }
               
            if (this.Selected) this.ShowCursor();
        
            //Draw Scroll Bar
            WindowManager.DrawColourBlock(ConsoleColor.White, this.Xpostion, this.Ypostion + this.Width, this.Xpostion + this.Height, this.Ypostion + this.Width + 1);
            
            double linesPerPixel = (double)lines.Count() / (this.Height);
            int postion = 0;
            if (linesPerPixel > 0)
            {
                postion = (int)Math.Floor(this.cursorDisplayX / linesPerPixel);
            }

            WindowManager.WirteText("■", this.Xpostion + postion, this.Ypostion + this.Width, ConsoleColor.DarkGray, ConsoleColor.White);
        }

        private List<string> CreateSplitText()
        {
            List<string> splitText = new List<string>();
            
            int lastSplit = 0;
            for (int i = 0; i < this.Text.Count() + 1; i++)
            {
                if (this.Text.Count() > i && this.Text[i] == '\n')
                {
                    splitText.Add(this.Text.Substring(lastSplit, i - lastSplit + 1));
                    lastSplit = i + 1;
                }
                else if (i - lastSplit == this.Width - 2)
                {
                    splitText.Add(this.Text.Substring(lastSplit, i - lastSplit));
                    lastSplit = i;
                }
                
                if (i == this.Text.Count())
                    splitText.Add(this.Text.Substring(lastSplit, this.Text.Count() - lastSplit));
            }

            return splitText.Select(x => x.Replace('\r', ' ')).ToList();
        }

        private void ShowCursor()
        {
            this.cursor.PlaceCursor(this.Xpostion + this.CursorDisplayX - this.Offset, this.Ypostion + 1 + this.CursorDisplayY, (this.Text + ' ')[this.CursorPostion], this.BackgroundColour);
        }

        private void UpdateCursorDisplayPostion()
        {
            List<string> lines = this.SplitText;
            int displayX = 0;
            int displayY = 0;

            for (int i = 0; i < this.CursorPostion; i++)
            {
                if (lines[displayX].Count() > displayY && lines[displayX][displayY] == '\n') //Skip NewLine characters
                {
                    displayY++;
                }

                if (lines.Count > displayX)
                {
                    if (lines[displayX].Count() > displayY)
                        displayY++;
                    else if (lines.Count - 1 > displayX)
                    {
                        displayX++;
                        displayY = 0;
                    }

                }

                if (displayY == 0 && displayX - 1 >= 0 && lines[displayX - 1].Last() != '\n') //Wordwrap Stuff
                {
                    displayY++;
                }
                else if (displayY == 1 && displayX - 1 >= 0 && lines[displayX - 1].Last() != '\n')
                {
                    displayY--;
                }
                
            }

            this.CursorDisplayX = displayX;
            this.CursorDisplayY = displayY;
        }

        private void RemoveCursor()
        {
            this.cursor.RemoveCursor();
        }

        private void SetOffset()
        {
            while (this.CursorDisplayX - this.Offset > this.Height - 1) this.Offset++;

            while (this.CursorDisplayX - this.Offset < 0) this.Offset--;
        }

        private string RemoveNewLine(string text)
        {
            string toReturn = "";

            foreach (char letter in text)
            {
                if (letter != '\n')
                    toReturn += letter;
            }

            return toReturn;
        }
    }
}
