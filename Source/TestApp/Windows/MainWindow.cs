namespace TestApp.Windows
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using ConsoleDraw.Inputs;
    using ConsoleDraw.Windows;
    using ConsoleDraw.Windows.Base;

    public class MainWindow : FullWindow
    {
        public MainWindow()
            : base(0, 0, Console.WindowWidth, Console.WindowHeight, null)
        {
            Button oneBtn = new Button(2, 2, "Button One", "oneBtn", this) { Action = () => new Alert("You Clicked Button One", this, ConsoleColor.White) };
            Button twoBtn = new Button(4, 2, "Button Two", "twoBtn", this) { Action = () => new Alert("You Clicked Button Two", this, ConsoleColor.White) };
            Button threeBtn = new Button(6, 2, "Long Alert", "threeoBtn", this) { Action = () => new Alert("A web browser (commonly referred to as a browser) is a software application for retrieving, presenting and traversing information resources on the World Wide", this, ConsoleColor.White) };

            Button displayAlertBtn = new Button(2, 20, "Display Alert", "displayAlertBtn", this) { Action = () => new Alert("This is an Alert!", this, ConsoleColor.White) };
            Button displayConfirmBtn = new Button(4, 20, "Display Confirm", "displayConfirmBtn", this) { Action = () => new Confirm("This is a Confirm!", this, ConsoleColor.White) };
            Button exitBtn = new Button(6, 20, "Exit", "exitBtn", this) { Action = () => this.ExitWindow() };
            Button displaySettingBtn = new Button(2, 40, "Display Settings", "displaySettingsBtn", this) { Action = () => new SettingsWindow(this) };

            Button displaySaveBtn = new Button(4, 40, "Display Save Menu", "displaySaveMenuBtn", this) { Action = () => new SaveMenu("Untitled.txt", Directory.GetCurrentDirectory(), "Test Data", this) };
            Button displayLoadBtn = new Button(6, 40, "Display Load Menu", "displayLoadMenuBtn", this) { Action = () => new LoadMenu(Directory.GetCurrentDirectory(), new Dictionary<string, string> { { "txt", "Text Document" }, { "*", "All Files" } }, this) };

            CheckBox oneCheckBox = new CheckBox(10, 2, "oneCheckBox", this);
            Label oneCheckBoxLabel = new Label("Check Box One", 10, 6, "oneCheckBoxLabel", this);
            CheckBox twoCheckBox = new CheckBox(12, 2, "twoCheckBox", this) { Checked = true };
            Label twoCheckBoxLabel = new Label("Check Box Two", 12, 6, "twoCheckBoxLabel", this);
            CheckBox threeCheckBox = new CheckBox(14, 2, "threeCheckBox", this);
            Label threeCheckBoxLabel = new Label("Check Box Three", 14, 6, "threeCheckBoxLabel", this);

            Label groupOneLabel = new Label("Radio Button Group One", 9, 25, "oneCheckBoxLabel", this);
            RadioButton oneRadioBtnGroupOne = new RadioButton(10, 25, "oneRadioBtnGroupOne", "groupOne", this) { Checked = true };
            Label oneRadioBtnGroupOneLabel = new Label("Radio Button One", 10, 29, "oneCheckBoxLabel", this);
            RadioButton twoRadioBtnGroupOne = new RadioButton(12, 25, "twoRadioBtnGroupOne", "groupOne", this);
            Label twoRadioBtnGroupOneLabel = new Label("Radio Button Two", 12, 29, "oneCheckBoxLabel", this);
            RadioButton threeRadioBtnGroupOne = new RadioButton(14, 25, "threeRadioBtnGroupOne", "groupOne", this);
            Label threeRadioBtnGroupOneLabel = new Label("Radio Button Three", 14, 29, "oneCheckBoxLabel", this);

            Label groupTwoLabel = new Label("Radio Button Group Two", 9, 50, "oneCheckBoxLabel", this);
            RadioButton oneRadioBtnGroupTwo = new RadioButton(10, 50, "oneRadioBtnGroupTwo", "groupTwo", this) { Checked = true };
            RadioButton twoRadioBtnGroupTwo = new RadioButton(12, 50, "twoRadioBtnGroupTwo", "groupTwo", this);
            RadioButton threeRadioBtnGroupTwo = new RadioButton(14, 50, "threeRadioBtnGroupTwo", "groupTwo", this);

            Label textAreaLabel = new Label("Text Area", 16, 2, "textAreaLabel", this);
            TextArea textArea = new TextArea(17, 2, 60, 6, "txtArea", this);
            textArea.BackgroundColour = ConsoleColor.DarkGray;

            Label txtBoxLabel = new Label("Text Box", 24, 2, "txtBoxLabel", this);
            TextBox txtBox = new TextBox(24, 11, "txtBox", this);

            FileBrowser fileSelect = new FileBrowser(26, 2, 40, 10, Directory.GetCurrentDirectory(), "fileSelect", this, true);

            this.Inputs.Add(oneBtn);
            this.Inputs.Add(twoBtn);
            this.Inputs.Add(threeBtn);
            this.Inputs.Add(oneCheckBox);
            this.Inputs.Add(oneCheckBoxLabel);
            this.Inputs.Add(twoCheckBox);
            this.Inputs.Add(twoCheckBoxLabel);
            this.Inputs.Add(threeCheckBox);
            this.Inputs.Add(threeCheckBoxLabel);

            this.Inputs.Add(displayAlertBtn);
            this.Inputs.Add(displayConfirmBtn);
            this.Inputs.Add(exitBtn);

            this.Inputs.Add(groupOneLabel);
            this.Inputs.Add(oneRadioBtnGroupOne);
            this.Inputs.Add(oneRadioBtnGroupOneLabel);
            this.Inputs.Add(twoRadioBtnGroupOne);
            this.Inputs.Add(twoRadioBtnGroupOneLabel);
            this.Inputs.Add(threeRadioBtnGroupOne);
            this.Inputs.Add(threeRadioBtnGroupOneLabel);

            this.Inputs.Add(displaySettingBtn);
            this.Inputs.Add(displaySaveBtn);
            this.Inputs.Add(displayLoadBtn);

            this.Inputs.Add(groupTwoLabel);
            this.Inputs.Add(oneRadioBtnGroupTwo);
            this.Inputs.Add(twoRadioBtnGroupTwo);
            this.Inputs.Add(threeRadioBtnGroupTwo);

            this.Inputs.Add(textAreaLabel);
            this.Inputs.Add(textArea);

            this.Inputs.Add(txtBoxLabel);
            this.Inputs.Add(txtBox);

            this.Inputs.Add(fileSelect);

            this.CurrentlySelected = oneBtn;

            this.Draw();
            this.MainLoop();
        }
    }
}