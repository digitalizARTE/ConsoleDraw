using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class Dropdown : Input
    {
        private ConsoleColor TextColour = ConsoleColor.Black;
        private ConsoleColor BackgroudColour = ConsoleColor.Gray;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;

        private bool Selected = false;
        public List<DropdownItem> DropdownItems = new List<DropdownItem>();
        public DropdownSpread DropdownSpread;

        private List<string> Options;
        public string Text;
        public int Length;

        public Action OnUnselect;

        public Dropdown(int x, int y, List<string> options, string iD, Window parentWindow, int length = 20) : base(x, y, 1, length - 2 + 3, parentWindow, iD)
        {
            this.Xpostion = x;
            this.Ypostion = y;
            this.Options = options;
            this.Text = this.Options.First();
            this.Length = length;
            this.BackgroudColour = parentWindow.BackgroundColour;

            this.Selectable = true;
        }

        public override void Draw()
        {
            string paddedText = this.Text.PadRight(this.Length - 2, ' ').Substring(0, this.Length - 2);

            if (this.Selected)
                WindowManager.WirteText('[' + paddedText + '▼' + ']', this.Xpostion, this.Ypostion, this.SelectedTextColour, this.SelectedBackgroundColour);
            else
                WindowManager.WirteText('[' + paddedText + '▼' + ']', this.Xpostion, this.Ypostion, this.TextColour, this.BackgroudColour);
        }

        public override void Select()
        {
            if (!this.Selected)
            {
                this.Selected = true;
                this.Draw();

                new DropdownSpread(this.Xpostion + 1, this.Ypostion, this.Options, this.ParentWindow, this);
            }
        }

        public override void Unselect()
        {
            if (this.Selected)
            {
                this.Selected = false;
                this.Draw();
                if(this.OnUnselect != null) this.OnUnselect();
            }
        }

        
    }
}
