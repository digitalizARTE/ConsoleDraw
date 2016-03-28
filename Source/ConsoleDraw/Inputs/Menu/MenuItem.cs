using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class MenuItem : Input
    {
        private string Text = string.Empty;
        private ConsoleColor TextColour = ConsoleColor.White;
        private ConsoleColor BackgroudColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.Black;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private bool Selected = false;
        public Action Action;

        public MenuItem(string text, string iD, Window parentWindow)
            : base(0, 0, 1, 0, parentWindow, iD)
        {
            this.Text = text;

            this.Selectable = true;
        }

        public override void Draw()
        {
            string paddedText = ('[' + this.Text + ']').PadRight(this.Width, ' ');

            if (this.Selected)
            {
                WindowManager.WirteText(
                    paddedText,
                    this.Xpostion,
                    this.Ypostion,
                    this.SelectedTextColour,
                    this.SelectedBackgroundColour);
            }
            else
            {
                WindowManager.WirteText(paddedText, this.Xpostion, this.Ypostion, this.TextColour, this.BackgroudColour);
            }
        }

        public override void Select()
        {
            if (!this.Selected)
            {
                this.Selected = true;
                this.Draw();

               // new MenuDropdown(Xpostion + 1, Ypostion, ParentWindow);
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
            //ParentWindow.ParentWindow.MoveToNextItem();
            this.ParentWindow.SelectFirstItem();
            this.ParentWindow.ExitWindow();
        }

        public override void Enter()
        {
            if (this.Action != null)
            {
                this.ParentWindow.SelectFirstItem();
                this.Action();
            }
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
            this.ParentWindow.SelectFirstItem();
            this.ParentWindow.ExitWindow();
            this.ParentWindow.ParentWindow.MoveToNextItem();
        }

        public override void CursorMoveLeft()
        {
            this.ParentWindow.SelectFirstItem();
            this.ParentWindow.ExitWindow();
            this.ParentWindow.ParentWindow.MoveToLastItem();
        }
    }
}
