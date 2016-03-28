using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class RadioButton : Input
    {
        public ConsoleColor BackgroundColour = ConsoleColor.Gray;
        private ConsoleColor TextColour = ConsoleColor.Black;

        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;

        private bool Selected = false;
        public bool Checked = false;
        public string RadioGroup;

        public Action Action;

        public RadioButton(int x, int y, string iD, string radioGroup, Window parentWindow)
            : base(x, y, 1, 3, parentWindow, iD)
        {
            this.RadioGroup = radioGroup;
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
            if (this.Checked) //Already checked, no need to change
                return;

            //Uncheck all other Radio Buttons in the group
            this.ParentWindow.Inputs.OfType<RadioButton>().Where(x => x.RadioGroup == this.RadioGroup).ToList().ForEach(x => x.Uncheck());

            this.Checked = true;

            this.Draw();

            if (this.Action != null) //If an action has been set
                this.Action();
        }

        public void Uncheck()
        {
            if (!this.Checked) //Already unchecked, no need to change
                return;

            this.Checked = false;
            this.Draw();
        }

        public override void Draw()
        {
            string Char = this.Checked ? "■" : " ";
            string text = string.Format("[{0}]", Char);
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
                WindowManager.WirteText(text, this.Xpostion, this.Ypostion, this.TextColour, this.BackgroundColour);
            }
        }

        public override void CursorMoveDown()
        {
            this.ParentWindow.MovetoNextItemDown(this.Xpostion + 1, this.Ypostion, this.Width);
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
            this.ParentWindow.MovetoNextItemUp(this.Xpostion - 1, this.Ypostion, this.Width);
        }
    }
}
