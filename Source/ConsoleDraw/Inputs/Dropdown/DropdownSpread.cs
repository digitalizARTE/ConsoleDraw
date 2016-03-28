using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class DropdownSpread : FullWindow
    {
        private List<DropdownItem> DropdownItems = new List<DropdownItem>();
        public Dropdown root;

        public DropdownSpread(int Xpostion, int Ypostion, List<string> options, Window parentWindow, Dropdown root)
            : base(Xpostion, Ypostion, 20, options.Count(), parentWindow)
        {
            for (int i = 0; i < options.Count(); i++)
            {
                DropdownItem item = new DropdownItem(options[i], Xpostion + i, "option" + i, this);

                item.Action = delegate() {
                    root.Text = ((DropdownItem)this.CurrentlySelected).Text;
                    root.Draw();
                };

                this.DropdownItems.Add(item);
            }

            this.Inputs.AddRange(this.DropdownItems);

            this.CurrentlySelected = this.DropdownItems.FirstOrDefault(x => x.Text == root.Text);

            this.BackgroundColour = ConsoleColor.DarkGray;
            this.Draw();
            this.MainLoop();
        }
    }
}
