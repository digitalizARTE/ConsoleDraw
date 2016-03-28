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
    public class LoadMenu : PopupWindow
    {
        private Button loadBtn;
        private Button cancelBtn;
        private TextBox openTxtBox;
        private FileBrowser fileSelect;
        private Dropdown fileTypeDropdown;

        public bool DataLoaded;
        public string Data;
        public string FileNameLoaded;
        public string PathOfLoaded;
        public Dictionary<string, string> FileTypes;

        public LoadMenu(string path, Dictionary<string, string> fileTypes, Window parentWindow)
            : base("Load Menu", Math.Min(6, Console.WindowHeight - 22), (Console.WindowWidth / 2) - 30, 60, 20, parentWindow)
        {
            this.BackgroundColour = ConsoleColor.White;
            this.FileTypes = fileTypes;

            this.fileSelect = new FileBrowser(this.PostionX + 2, this.PostionY + 2, 56, 13, path, "fileSelect", this, true, "txt") { ChangeItem = this.UpdateCurrentlySelectedFileName, SelectFile = this.LoadFile };

            Label openLabel = new Label("Open", this.PostionX + 16, this.PostionY + 2, "openLabel", this);
            this.openTxtBox = new TextBox(this.PostionX + 16, this.PostionY + 7, "openTxtBox", this, this.Width - 13) { Selectable = false };

            this.fileTypeDropdown = new Dropdown(
                this.PostionX + 18,
                this.PostionY + 40,
                this.FileTypes.Select(x => x.Value).ToList(),
                "fileTypeDropdown",
                this,
                17) { OnUnselect = this.UpdateFileTypeFilter };

            this.loadBtn = new Button(this.PostionX + 18, this.PostionY + 2, "Load", "loadBtn", this) { Action = this.LoadFile };
            this.cancelBtn = new Button(this.PostionX + 18, this.PostionY + 9, "Cancel", "cancelBtn", this) { Action = this.ExitWindow };
            this.Inputs.Add(this.fileSelect);
            this.Inputs.Add(this.loadBtn);
            this.Inputs.Add(this.cancelBtn);
            this.Inputs.Add(openLabel);
            this.Inputs.Add(this.openTxtBox);
            this.Inputs.Add(this.fileTypeDropdown);

            this.CurrentlySelected = this.fileSelect;

            this.Draw();
            this.MainLoop();
        }

        private void UpdateCurrentlySelectedFileName()
        {
            string currentlySelectedFile = this.fileSelect.CurrentlySelectedFile;
            this.openTxtBox.SetText(currentlySelectedFile);
        }

        private void UpdateFileTypeFilter()
        {
            KeyValuePair<string, string> filter = this.FileTypes.FirstOrDefault(x => x.Value == this.fileTypeDropdown.Text);
            KeyValuePair<string, string> currentFilter = this.FileTypes.FirstOrDefault(x => x.Key == this.fileSelect.FilterByExtension);
            if (currentFilter.Key != filter.Key)
            {
                this.fileSelect.FilterByExtension = filter.Key;
                this.fileSelect.GetFileNames();
                this.fileSelect.Draw();
            }
        }

        private void LoadFile()
        {
            if (this.fileSelect.CurrentlySelectedFile == string.Empty)
            {
                new Alert("No file Selected", this, "Warning");
                return;
            }

            string file = Path.Combine(this.fileSelect.CurrentPath, this.fileSelect.CurrentlySelectedFile);
            string text = File.ReadAllText(file);

            /*var mainWindow = (MainWindow)ParentWindow;
            mainWindow.textArea.SetText(text);
            mainWindow.fileLabel.SetText(fileSelect.CurrentlySelectedFile);*/

            this.DataLoaded = true;
            this.Data = text;
            this.FileNameLoaded = this.fileSelect.CurrentlySelectedFile;
            this.PathOfLoaded = this.fileSelect.CurrentPath;

            this.ExitWindow();
        }
    }
}
