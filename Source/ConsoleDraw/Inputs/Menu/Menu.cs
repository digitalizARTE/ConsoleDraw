using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class Menu : Input
    {
        private string Text = string.Empty;
        private ConsoleColor TextColour = ConsoleColor.Black;
        private ConsoleColor BackgroudColour = ConsoleColor.Gray;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;

        private bool Selected = false;
        public List<MenuItem> MenuItems = new List<MenuItem>();
        public MenuDropdown MenuDropdown;

        public Menu(string text, int x, int y, string iD, Window parentWindow)
            : base(x, y, 1, text.Count() + 2, parentWindow, iD)
        {
            this.Text = text;
            this.Xpostion = x;
            this.Ypostion = y;

            this.Selectable = true;
        }

        public override void Draw()
        {
            string text = string.Format("{0}{1}{2}", '[', this.Text, ']');
            if (this.Selected)
            {
                WindowManager.WirteText(
                    text,
                    this.Xpostion,
                    this.Ypostion,
                    this.SelectedTextColour,
                    this.SelectedBackgroundColour);
            }
            else
            {
                WindowManager.WirteText(text, this.Xpostion, this.Ypostion, this.TextColour, this.BackgroudColour);
            }
        }

        public override void Select()
        {
            if (!this.Selected)
            {
                this.Selected = true;
                this.Draw();

                new MenuDropdown(this.Xpostion + 1, this.Ypostion, this.MenuItems, this.ParentWindow);

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
            this.MenuDropdown = new MenuDropdown(this.Xpostion + 1, this.Ypostion, this.MenuItems, this.ParentWindow);
        }

        public override void CursorMoveLeft()
        {
            this.ParentWindow.MoveToLastItem();
        }
        public override void CursorMoveRight()
        {
            this.ParentWindow.MoveToNextItem();
        }

        public override void CursorMoveDown()
        {
            this.MenuDropdown = new MenuDropdown(this.Xpostion + 1, this.Ypostion, this.MenuItems, this.ParentWindow);
        }
    }
}
