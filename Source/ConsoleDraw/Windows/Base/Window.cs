using ConsoleDraw.Inputs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Windows.Base
{
    public class Window : IWindow
    {
        public bool Exit;
        protected IInput CurrentlySelected;

        public int PostionX { get; private set; }
        public int PostionY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ConsoleColor BackgroundColour = ConsoleColor.Gray;

        public List<IInput> Inputs = new List<IInput>();

        public Window(int postionX, int postionY, int width, int height, Window parentWindow)
        {
            this.PostionY = postionY;
            this.PostionX = postionX;
            this.Width = width;
            this.Height = height;

            this.ParentWindow = parentWindow;
        }

        public void Draw()
        {
            if (this.ParentWindow != null)
            {
                this.ParentWindow.Draw();
            }

            this.ReDraw();

            foreach (IInput input in this.Inputs)
            {
                input.Draw();
            }

            if (this.CurrentlySelected != null)
            {
                this.CurrentlySelected.Select();
            }
            // SetSelected();
        }

        public override void ReDraw()
        {

        }
        
        public void MainLoop()
        {
            while (!this.Exit && !ProgramInfo.ExitProgram)
            {
                ConsoleKeyInfo input = ReadKey();
                if (input.Key == ConsoleKey.Tab)
                {
                    if ((input.Modifiers & ConsoleModifiers.Shift) != 0)
                    {
                        this.CurrentlySelected.UnTab();
                    }
                    else
                    {
                        this.CurrentlySelected.Tab();
                    }
                }
                else if (input.Key == ConsoleKey.Enter)
                {
                    this.CurrentlySelected.Enter();
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {
                    this.CurrentlySelected.CursorMoveLeft();
                }
                else if (input.Key == ConsoleKey.RightArrow)
                {
                    this.CurrentlySelected.CursorMoveRight();
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                    this.CurrentlySelected.CursorMoveUp();
                }
                else if (input.Key == ConsoleKey.DownArrow)
                {
                    this.CurrentlySelected.CursorMoveDown();
                }
                else if (input.Key == ConsoleKey.Backspace)
                {
                    this.CurrentlySelected.BackSpace();
                }
                else if (input.Key == ConsoleKey.Home)
                {
                    this.CurrentlySelected.CursorToStart();
                }
                else if (input.Key == ConsoleKey.End)
                {
                    this.CurrentlySelected.CursorToEnd();
                }
                else
                {
                    this.CurrentlySelected.AddLetter((char)input.KeyChar); // Letter(input.KeyChar);
                }
            }
        }

        public void SelectFirstItem()
        {
            if (this.Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            this.CurrentlySelected = this.Inputs.First(x => x.Selectable);

            this.SetSelected();
        }

        public void SelectItemByID(string Id)
        {
            if (this.Inputs.All(x => !x.Selectable))
            {
                //No Selectable inputs on page
                return;
            }

            IInput newSelectedInput = this.Inputs.FirstOrDefault(x => x.ID == Id);
            if (newSelectedInput == null)
            {
                //No Input with this ID
                return;
            }

            this.CurrentlySelected = newSelectedInput;

            this.SetSelected();
        }

        public void MoveToNextItem()
        {
            if (this.Inputs.All(x => !x.Selectable))
            {
                //No Selectable inputs on page
                return;
            }

            if (this.Inputs.Count(x => x.Selectable) == 1)
            {
                //Only one selectable input on page, thus no point chnaging it
                return;
            }

            int IndexOfCurrent = this.Inputs.IndexOf(this.CurrentlySelected);

            while (true)
            {
                IndexOfCurrent = this.MoveIndexAlongOne(IndexOfCurrent);

                if (this.Inputs[IndexOfCurrent].Selectable)
                {
                    break;
                }
            }

            this.CurrentlySelected = this.Inputs[IndexOfCurrent];

            this.SetSelected();
        }

        public void MoveToPrevItem()
        {
            if (this.Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (this.Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            int IndexOfCurrent = this.Inputs.IndexOf(this.CurrentlySelected);

            while (true)
            {
                IndexOfCurrent = this.MoveIndexBackOne(IndexOfCurrent);

                if (this.Inputs[IndexOfCurrent].Selectable)
                    break;
            }

            this.CurrentlySelected = this.Inputs[IndexOfCurrent];

            this.SetSelected();
        }

        public void MoveToLastItem()
        {
            if (this.Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (this.Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            int IndexOfCurrent = this.Inputs.IndexOf(this.CurrentlySelected);

            while (true)
            {
                IndexOfCurrent = this.MoveIndexBackOne(IndexOfCurrent);

                if (this.Inputs[IndexOfCurrent].Selectable)
                    break;
            }
            this.CurrentlySelected = this.Inputs[IndexOfCurrent];

            this.SetSelected();
        }

        public void MovetoNextItemRight(int startX, int startY, int searchHeight)
        {
            if (this.Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (this.Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            IInput nextItem = null;
            while (nextItem == null && startY <= this.PostionY + this.Width)
            {
                foreach (IInput input in this.Inputs.Where(x => x.Selectable && x != this.CurrentlySelected))
                {
                    bool overlap = this.DoAreasOverlap(startX, startY, searchHeight, 1, input.Xpostion, input.Ypostion, input.Height, input.Width);
                    if (overlap)
                    {
                        nextItem = input;
                        break; //end foreach 
                    }
                }
                startY++;
            }

            if (nextItem == null) //No element found to the right
            {
                this.MoveToNextItem();
                return;
            }

            this.CurrentlySelected = nextItem;
            this.SetSelected();
        }

        public void MovetoNextItemLeft(int startX, int startY, int searchHeight)
        {
            if (this.Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (this.Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            IInput nextItem = null;
            while (nextItem == null && startY > this.PostionY)
            {
                foreach (IInput input in this.Inputs.Where(x => x.Selectable && x != this.CurrentlySelected))
                {
                    bool overlap = this.DoAreasOverlap(startX, startY - 1, searchHeight, 1, input.Xpostion, input.Ypostion, input.Height, input.Width);
                    if (overlap)
                    {
                        nextItem = input;
                        break; //end foreach 
                    }
                }
                startY--;
            }

            if (nextItem == null) //No element found
            {
                this.MoveToLastItem();
                return;
            }

            this.CurrentlySelected = nextItem;
            this.SetSelected();
        }

        public void MovetoNextItemDown(int startX, int startY, int searchWidth)
        {
            if (this.Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (this.Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            IInput nextItem = null;
            while (nextItem == null && startX <= this.PostionX + this.Height)
            {
                foreach (IInput input in this.Inputs.Where(x => x.Selectable && x != this.CurrentlySelected))
                {
                    bool overlap = this.DoAreasOverlap(startX, startY, 1, searchWidth, input.Xpostion, input.Ypostion, input.Height, input.Width);
                    if (overlap)
                    {
                        nextItem = input;
                        break; //end foreach 
                    }
                }
                startX++;
            }

            if (nextItem == null) //No element found
            {
                this.MoveToNextItem();
                return;
            }

            this.CurrentlySelected = nextItem;
            this.SetSelected();
        }

        public void MovetoNextItemUp(int startX, int startY, int searchWidth)
        {
            if (this.Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (this.Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            IInput nextItem = null;
            while (nextItem == null && startX > this.PostionX)
            {
                foreach (IInput input in this.Inputs.Where(x => x.Selectable && x != this.CurrentlySelected))
                {
                    bool overlap = this.DoAreasOverlap(startX - 1, startY, 1, searchWidth, input.Xpostion, input.Ypostion, input.Height, input.Width);
                    if (overlap)
                    {
                        nextItem = input;
                        break; //end foreach 
                    }
                }
                startX--;
            }

            if (nextItem == null) //No element found
            {
                this.MoveToLastItem();
                return;
            }

            this.CurrentlySelected = nextItem;
            this.SetSelected();
        }


        private bool DoAreasOverlap(int areaOneX, int areaOneY, int areaOneHeight, int areaOneWidth, int areaTwoX, int areaTwoY, int areaTwoHeight, int areaTwoWidth)
        {
            int areaOneEndX = areaOneX + areaOneHeight - 1;
            int areaOneEndY = areaOneY + areaOneWidth - 1;
            int areaTwoEndX = areaTwoX + areaTwoHeight - 1;
            int areaTwoEndY = areaTwoY + areaTwoWidth - 1;

            bool overlapsVertically = false;
            //Check if overlap vertically
            if (areaOneX >= areaTwoX && areaOneX < areaTwoEndX) //areaOne starts in areaTwo
                overlapsVertically = true;
            else if (areaOneEndX >= areaTwoX && areaOneEndX <= areaTwoEndX) //areaOne ends in areaTwo
                overlapsVertically = true;
            else if (areaOneX < areaTwoX && areaOneEndX >= areaTwoEndX) //areaOne start before and end after areaTwo
                overlapsVertically = true;
            //areaOne inside areaTwo is caught by first two statements

            if (!overlapsVertically) //If it does not overlap vertically, then it does not overlap.
                return false;

            bool overlapsHorizontally = false;
            //Check if overlap Horizontally
            if (areaOneY >= areaTwoY && areaOneY < areaTwoEndY) //areaOne starts in areaTwo
                overlapsHorizontally = true;
            else if (areaOneEndY >= areaTwoY && areaOneEndY < areaTwoEndY) //areaOne ends in areaTwo
                overlapsHorizontally = true;
            else if (areaOneY <= areaTwoY && areaOneEndY >= areaTwoEndY) //areaOne starts before and ends after areaTwo
                overlapsHorizontally = true;
            //areaOne inside areaTwo is caught by first two statements

            if (!overlapsHorizontally) //If it does not overlap Horizontally, then it does not overlap.
                return false;

            return true; //it overlaps vertically and horizontally, thus areas must overlap
        }

        private int MoveIndexAlongOne(int index)
        {
            if (this.Inputs.Count() == index + 1)
                return 0;

            return index + 1;
        }

        private int MoveIndexBackOne(int index)
        {
            if (index == 0)
                return this.Inputs.Count() - 1;

            return index - 1;
        }

        private void SetSelected()
        {
            this.Inputs.ForEach(x => x.Unselect());

            if (this.CurrentlySelected != null) this.CurrentlySelected.Select();
        }

        private static ConsoleKeyInfo ReadKey()
        {
            ConsoleKeyInfo input = Console.ReadKey(true);

            return input;
        }

        public IInput GetInputById(string Id)
        {
            return this.Inputs.FirstOrDefault(x => x.ID == Id);
        }

        public void ExitWindow()
        {
            this.Exit = true;
            if (this.ParentWindow != null)
            {
                this.ParentWindow.Draw();
            }
            //else
            //System.Environment.Exit(1);
        }
    }
}
