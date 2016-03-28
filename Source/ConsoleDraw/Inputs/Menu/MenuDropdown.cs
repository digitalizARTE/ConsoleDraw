using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class MenuDropdown : FullWindow
    {
        private readonly List<MenuItem> menuItems;

        public MenuDropdown(int xpostion, int ypostion, List<MenuItem> menuItems, Window parentWindow)
            : base(xpostion, ypostion, 20, menuItems.Count() + 2, parentWindow)
        {
            for (int i = 0; i < menuItems.Count(); i++)
            {
                menuItems[i].ParentWindow = this;
                menuItems[i].Width = this.Width - 2;
                menuItems[i].Xpostion = xpostion + i + 1;
                menuItems[i].Ypostion = this.PostionY + 1;
            }

            this.menuItems = menuItems;

            this.Inputs.AddRange(this.menuItems);

            this.CurrentlySelected = this.menuItems.FirstOrDefault();

            this.BackgroundColour = ConsoleColor.DarkGray;
            this.Draw();
            this.MainLoop();
        }
    }
}
