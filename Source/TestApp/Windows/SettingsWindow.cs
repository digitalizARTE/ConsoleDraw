using ConsoleDraw.Inputs;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Windows
{
    public class SettingsWindow : PopupWindow
    {
        public SettingsWindow(Window parentWindow)
            : base("Settings", 6, 10, 80, 20, parentWindow)
        {
            this.BackgroundColour = ConsoleColor.White;

            Label appTitleLabel = new Label("App Title", 8, 12, "appTitleLabel", this);
            TextBox appTitleTxtBox = new TextBox(8, 25, Console.Title, "appTitleTxtBox", this, 10);

            Label saveOnExitLabel = new Label("Save On Exit", 10, 12, "saveOnExitLabel", this);
            CheckBox saveOneExitChkBox = new CheckBox(10, 25, "saveOnExitCheckBox", this);

            Label byAllLabel = new Label("For All", 12, 12, "forAll", this);
            RadioButton byAllRadioBtn = new RadioButton(12, 25, "byAllRadioBtn", "Users", this) { Checked = true };
            Label justYouLabel = new Label("Just You", 14, 12, "justYou", this);
            RadioButton justYouRadioBtn = new RadioButton(14, 25, "justYouRadioBtn", "Users", this);

            Button applyBtn = new Button(24, 12, "Apply", "exitBtn", this) { Action = () => this.ExitWindow() };
            Button exitBtn = new Button(24, 20, "Exit", "exitBtn", this) { Action = () => this.ExitWindow() };

            this.Inputs.Add(appTitleLabel);
            this.Inputs.Add(appTitleTxtBox);

            this.Inputs.Add(saveOnExitLabel);
            this.Inputs.Add(saveOneExitChkBox);

            this.Inputs.Add(byAllLabel);
            this.Inputs.Add(byAllRadioBtn);
            this.Inputs.Add(justYouLabel);
            this.Inputs.Add(justYouRadioBtn);

            this.Inputs.Add(applyBtn);
            this.Inputs.Add(exitBtn);

            this.CurrentlySelected = exitBtn;

            this.Draw();
            this.MainLoop();
        }


    }
}
