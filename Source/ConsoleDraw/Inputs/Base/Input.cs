using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs.Base
{
    public class Input : IInput
    {
        public Input(int xPostion, int yPostion, int height, int width, Window parentWindow, string iD)
        {
            this.ParentWindow = parentWindow;
            this.ID = iD;

            this.Xpostion = xPostion;
            this.Ypostion = yPostion;

            this.Height = height;
            this.Width = width;
        }

        public override void AddLetter(char letter) { }
        public override void BackSpace() { }
        public override void CursorMoveLeft() { }
        public override void CursorMoveRight() { }
        public override void CursorMoveUp() { }
        public override void CursorMoveDown() { }
        public override void CursorToStart() { }
        public override void CursorToEnd() { }
        public override void Enter() { }
        public override void Tab() {
            this.ParentWindow.MoveToNextItem();
        }

        public override void UnTab()
        {
            this.ParentWindow.MoveToPrevItem();
        }
        
        public override void Unselect() { }
        public override void Select() { }
        public override void Draw() { }
    }
}
