using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class DropdownItem : Input
    {
        public string Text = "";
        private ConsoleColor TextColour = ConsoleColor.White;
        private ConsoleColor BackgroudColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.Black;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private bool Selected = false;
        public Action Action;

        public DropdownItem(string text, int x, string iD, Window parentWindow) : base(x, parentWindow.PostionY + 1, 1, parentWindow.Width - 2, parentWindow, iD)
        {
            this.Text = text;

            this.Selectable = true;
        }

        public override void Draw()
        {
            string paddedText = (this.Text).PadRight(this.Width, ' ');

            if (this.Selected)
                WindowManager.WirteText(paddedText, this.Xpostion, this.Ypostion, this.SelectedTextColour, this.SelectedBackgroundColour);
            else
                WindowManager.WirteText(paddedText, this.Xpostion, this.Ypostion, this.TextColour, this.BackgroudColour);
        }

        public override void Select()
        {
            if (!this.Selected)
            {
                this.Selected = true;
                this.Draw();

                if (this.Action != null) this.Action();
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

        public override void BackSpace()
        {
            this.ParentWindow.SelectFirstItem();
            this.ParentWindow.ExitWindow();
        }

        public override void CursorMoveDown()
        {
            this.ParentWindow.MoveToNextItem();
        }
        public override void CursorMoveUp()
        {
            this.ParentWindow.MoveToLastItem();
        }

        public override void CursorMoveRight()
        {
            this.ParentWindow.ExitWindow();
            this.ParentWindow.ParentWindow.MoveToNextItem();
        }

        public override void CursorMoveLeft()
        {
            this.ParentWindow.ExitWindow();
            this.ParentWindow.ParentWindow.MoveToLastItem();
        }
    }
}
