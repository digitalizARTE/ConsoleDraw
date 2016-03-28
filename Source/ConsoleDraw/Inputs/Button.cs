using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class Button : Input
    {
        private string Text;
        public ConsoleColor BackgroundColour = ConsoleColor.Gray;
        private ConsoleColor TextColour = ConsoleColor.Black;

        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;

        private bool Selected = false;

        public Action Action;

        public Button(int x, int y, string text, string iD, Window parentWindow) : base(x, y, 1, text.Count() + 2, parentWindow, iD)
        {
            this.Text = text;
            this.BackgroundColour = parentWindow.BackgroundColour;
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
            if (this.Action != null) //If an action has been set
                this.Action();
        }

        public override void Draw()
        {
            if(this.Selected)
                WindowManager.WirteText('['+ this.Text+']', this.Xpostion, this.Ypostion, this.SelectedTextColour, this.SelectedBackgroundColour);
            else
                WindowManager.WirteText('[' + this.Text + ']', this.Xpostion, this.Ypostion, this.TextColour, this.BackgroundColour);  
        }
        
        public override void CursorMoveDown()
        {
            this.ParentWindow.MovetoNextItemDown(this.Xpostion, this.Ypostion , this.Width);
        }

        public override void CursorMoveRight()
        {
            this.ParentWindow.MovetoNextItemRight(this.Xpostion - 1, this.Ypostion + this.Width, 3);

        }

        public override void CursorMoveLeft()
        {
            this.ParentWindow.MovetoNextItemLeft(this.Xpostion - 1, this.Ypostion, 3);
        }

        public override void CursorMoveUp()
        {
            this.ParentWindow.MovetoNextItemUp(this.Xpostion, this.Ypostion, this.Width);
        }
    }
}
