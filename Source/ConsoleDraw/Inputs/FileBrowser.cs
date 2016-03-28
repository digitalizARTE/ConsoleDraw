using ConsoleDraw.Inputs.Base;
using ConsoleDraw.Windows;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Inputs
{
    public class FileBrowser : Input
    {
        public string CurrentPath { get; private set; }
        public string CurrentlySelectedFile { get; private set; }
        private List<string> FileNames = new List<string>();
        private List<string> Folders;
        private List<string> Drives;

        public bool IncludeFiles;
        public string FilterByExtension = "*";

        private ConsoleColor BackgroundColour = ConsoleColor.DarkGray;
        private ConsoleColor TextColour = ConsoleColor.Black;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private int cursorX;
        private int CursorX
        {
            get { return this.cursorX; }
            set
            {
                this.cursorX = value;
                this.GetCurrentlySelectedFileName();
                this.SetOffset();
            }
        }

        private int Offset = 0;
        private bool Selected = false;
        private bool AtRoot = false;
        private bool ShowingDrive = false;

        public Action ChangeItem;
        public Action SelectFile;

        public FileBrowser(int x, int y, int width, int height, string path, string iD, Window parentWindow, bool includeFiles = false, string filterByExtension = "*")
            : base(x, y, height, width, parentWindow, iD)
        {
            this.CurrentPath = path;
            this.CurrentlySelectedFile = "";
            this.IncludeFiles = includeFiles;
            this.FilterByExtension = filterByExtension;
            this.Drives = Directory.GetLogicalDrives().ToList();

            this.GetFileNames();
            this.Selectable = true;
        }


        public override void Draw()
        {
            WindowManager.DrawColourBlock(this.BackgroundColour, this.Xpostion, this.Ypostion, this.Xpostion + this.Height, this.Ypostion + this.Width);

            if (!this.ShowingDrive)
            {
                string trimedPath = this.CurrentPath.PadRight(this.Width - 2, ' ');
                trimedPath = trimedPath.Substring(trimedPath.Count() - this.Width + 2, this.Width - 2);
                WindowManager.WirteText(
                    trimedPath,
                    this.Xpostion,
                    this.Ypostion + 1,
                    ConsoleColor.Gray,
                    this.BackgroundColour);
            }
            else
            {
                WindowManager.WirteText("Drives", this.Xpostion, this.Ypostion + 1, ConsoleColor.Gray, this.BackgroundColour);
            }

            if (!this.ShowingDrive)
            {
                int i = this.Offset;
                while (i < Math.Min(this.Folders.Count, this.Height + this.Offset - 1))
                {
                    string folderName = this.Folders[i].PadRight(this.Width - 2, ' ').Substring(0, this.Width - 2);
                    if (i == this.CursorX)
                    {
                        if (this.Selected)
                        {
                            WindowManager.WirteText(
                                folderName,
                                this.Xpostion + i - this.Offset + 1,
                                this.Ypostion + 1,
                                this.SelectedTextColour,
                                this.SelectedBackgroundColour);
                        }
                        else
                        {
                            WindowManager.WirteText(
                                folderName,
                                this.Xpostion + i - this.Offset + 1,
                                this.Ypostion + 1,
                                this.SelectedTextColour,
                                this.BackgroundColour);
                        }
                    }
                    else
                    {
                        WindowManager.WirteText(
                          folderName,
                          this.Xpostion + i - this.Offset + 1,
                          this.Ypostion + 1,
                          this.TextColour,
                          this.BackgroundColour);
                    }

                    i++;
                }

                while (i < Math.Min(this.Folders.Count + this.FileNames.Count, this.Height + this.Offset - 1))
                {
                    string fileName = this.FileNames[i - this.Folders.Count].PadRight(this.Width - 2, ' ').Substring(0, this.Width - 2);

                    if (i == this.CursorX)
                    {
                        if (this.Selected)
                        {
                            WindowManager.WirteText(
                                fileName,
                                this.Xpostion + i - this.Offset + 1,
                                this.Ypostion + 1,
                                this.SelectedTextColour,
                                this.SelectedBackgroundColour);
                        }
                        else
                        {
                            WindowManager.WirteText(
                                fileName,
                                this.Xpostion + i - this.Offset + 1,
                                this.Ypostion + 1,
                                this.SelectedTextColour,
                                this.BackgroundColour);
                        }
                    }
                    else
                    {
                        WindowManager.WirteText(
                            fileName,
                            this.Xpostion + i - this.Offset + 1,
                            this.Ypostion + 1,
                            this.TextColour,
                            this.BackgroundColour);
                    }
                    i++;
                }
            }
            else
            {
                for (int i = 0; i < this.Drives.Count(); i++)
                {
                    if (i == this.CursorX)
                    {
                        if (this.Selected)
                        {
                            WindowManager.WirteText(
                                this.Drives[i],
                                this.Xpostion + i - this.Offset + 1,
                                this.Ypostion + 1,
                                this.SelectedTextColour,
                                this.SelectedBackgroundColour);
                        }
                        else
                        {
                            WindowManager.WirteText(
                                this.Drives[i],
                                this.Xpostion + i - this.Offset + 1,
                                this.Ypostion + 1,
                                this.SelectedTextColour,
                                this.BackgroundColour);
                        }
                    }
                    else
                    {
                        WindowManager.WirteText(
                            this.Drives[i],
                            this.Xpostion + i - this.Offset + 1,
                            this.Ypostion + 1,
                            this.TextColour,
                            this.BackgroundColour);
                    }

                }

            }

        }

        public void GetFileNames()
        {
            if (this.ShowingDrive) //Currently Showing drives. This function should not be called!
                return;

            try
            {
                if (this.IncludeFiles)
                {
                    this.FileNames = Directory.GetFiles(this.CurrentPath, string.Format("*.{0}", this.FilterByExtension)).Select(path => Path.GetFileName(path)).ToList();
                }

                this.Folders = Directory.GetDirectories(this.CurrentPath).Select(path => Path.GetFileName(path)).ToList();

                this.Folders.Insert(0, "..");

                if (Directory.GetParent(this.CurrentPath) != null)
                {
                    this.AtRoot = false;

                }
                else
                {
                    this.AtRoot = true;
                }

                if (this.CursorX > this.FileNames.Count() + this.Folders.Count())
                {
                    this.CursorX = 0;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                // throw e;
                throw;
            }
        }

        private void DisplayDrives()
        {
            this.ShowingDrive = true;
            this.CurrentPath = "";
            this.CursorX = 0;
            this.Draw();
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

        public override void CursorMoveDown()
        {
            if (this.CursorX != this.Folders.Count + this.FileNames.Count - 1 && !this.ShowingDrive)
            {
                this.CursorX++;
                this.Draw();
            }
            else if (this.CursorX != this.Drives.Count - 1 && this.ShowingDrive)
            {
                this.CursorX++;
                this.Draw();
            }
            else
            {
                this.ParentWindow.MovetoNextItemDown(this.Xpostion, this.Ypostion, this.Width);
            }
        }

        public override void CursorMoveUp()
        {
            if (this.CursorX != 0)
            {
                this.CursorX--;
                this.Draw();
            }
            else
            {
                this.ParentWindow.MovetoNextItemUp(this.Xpostion, this.Ypostion, this.Width);
            }
        }

        public override void CursorMoveRight()
        {
            if (this.CursorX >= 1 && this.CursorX < this.Folders.Count && !this.ShowingDrive)
            {
                //Folder is selected
                this.GoIntoFolder();
            }
            else if (this.ShowingDrive)
            {
                this.GoIntoDrive();
            }
        }

        public override void Enter()
        {
            if (this.CursorX >= 1 && this.CursorX < this.Folders.Count && !this.ShowingDrive)
            {
                //Folder is selected
                this.GoIntoFolder();
            }
            else if (this.cursorX == 0 && !this.AtRoot)
            {//Back is selected
                this.GoToParentFolder();
            }
            else if (this.SelectFile != null && !this.ShowingDrive)
            {
                //File is selcted
                this.SelectFile();
            }
            else if (this.cursorX == 0 && this.AtRoot && !this.ShowingDrive)
            {
                //Back is selected and at root, thus show drives
                this.DisplayDrives();
            }
            else if (this.ShowingDrive)
            {
                this.GoIntoDrive();
            }
        }

        private void GoIntoDrive()
        {
            this.CurrentPath = this.Drives[this.cursorX];

            try
            {
                this.ShowingDrive = false;
                this.GetFileNames();
                this.CursorX = 0;
                this.Draw();
            }
            catch (IOException e)
            {
                this.CurrentPath = ""; //Change Path back to nothing
                this.ShowingDrive = true;
                new Alert(e.Message, this.ParentWindow, ConsoleColor.White);
            }

        }

        private void GoIntoFolder()
        {
            this.CurrentPath = Path.Combine(this.CurrentPath, this.Folders[this.cursorX]);

            try
            {
                this.GetFileNames();
                this.CursorX = 0;
                this.Draw();
            }
            catch (UnauthorizedAccessException e)
            {
                this.CurrentPath = Directory.GetParent(this.CurrentPath).FullName; //Change Path back to parent
                new Alert("Access Denied", this.ParentWindow, ConsoleColor.White, "Error");
            }
        }

        public override void CursorMoveLeft()
        {
            if (!this.AtRoot)
            {
                this.GoToParentFolder();
            }
            else
            {
                this.DisplayDrives();
            }
        }

        public override void BackSpace()
        {
            if (!this.AtRoot)
            {
                this.GoToParentFolder();
            }
        }

        private void GoToParentFolder()
        {
            this.CurrentPath = Directory.GetParent(this.CurrentPath).FullName;
            this.GetFileNames();
            this.CursorX = 0;
            this.Draw();
        }

        private void SetOffset()
        {
            while (this.CursorX - this.Offset > this.Height - 2)
            {
                this.Offset++;
            }

            while (this.CursorX - this.Offset < 0)
            {
                this.Offset--;
            }
        }

        private void GetCurrentlySelectedFileName()
        {
            if (this.cursorX >= this.Folders.Count()) //File is selected
            {
                this.CurrentlySelectedFile = this.FileNames[this.cursorX - this.Folders.Count];
                if (this.ChangeItem != null)
                {
                    this.ChangeItem();
                }
            }
            else
            {
                if (this.CurrentlySelectedFile != "")
                {
                    this.CurrentlySelectedFile = "";
                    if (this.ChangeItem != null) this.ChangeItem();
                }
            }
        }
    }
}
