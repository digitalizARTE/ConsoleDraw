using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDraw.Windows.Base;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows;

namespace ConsoleDraw.Windows
{
    public class SaveMenu : PopupWindow
    {
        private readonly Button saveBtn;
        private readonly Button cancelBtn;
        private readonly TextBox openTxtBox;
        private readonly FileBrowser fileSelect;
        private readonly string text;

        public bool FileWasSaved;
        public string FileSavedAs;
        public string PathToFile;

        public SaveMenu(string fileName, string path, string data, Window parentWindow)
            : base("Save Menu", 6, (Console.WindowWidth / 2) - 30, 60, 20, parentWindow)
        {
            this.BackgroundColour = ConsoleColor.White;
            this.text = data;
            this.fileSelect = new FileBrowser(this.PostionX + 2, this.PostionY + 2, 56, 12, path, "fileSelect", this);
            Label openLabel = new Label("Name", this.PostionX + 16, this.PostionY + 2, "openLabel", this);
            this.openTxtBox = new TextBox(this.PostionX + 16, this.PostionY + 7, fileName, "openTxtBox", this, this.Width - 13) { Selectable = true };
            this.saveBtn = new Button(this.PostionX + 18, this.PostionY + 2, "Save", "loadBtn", this)
            {
                Action = this.SaveFile
            };
            this.cancelBtn = new Button(this.PostionX + 18, this.PostionY + 9, "Cancel", "cancelBtn", this)
            {
                Action = this.ExitWindow
            };

            this.Inputs.Add(this.fileSelect);
            this.Inputs.Add(openLabel);
            this.Inputs.Add(this.openTxtBox);
            this.Inputs.Add(this.saveBtn);
            this.Inputs.Add(this.cancelBtn);
            this.CurrentlySelected = this.saveBtn;
            this.Draw();
            this.MainLoop();
        }


        private void SaveFile()
        {
            string path = this.fileSelect.CurrentPath;
            string filename = this.openTxtBox.GetText();
            string fullFile = Path.Combine(path, filename);
            try
            {
                StreamWriter file = new StreamWriter(fullFile);
                file.Write(this.text);
                file.Close();

                this.FileWasSaved = true;
                this.FileSavedAs = filename;
                this.PathToFile = path;

                this.ExitWindow();
            }
            catch
            {
                new Alert("You do not have access", this, "Error");
            }
        }
    }
}
