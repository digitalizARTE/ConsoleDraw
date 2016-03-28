using ConsoleDraw.Inputs;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Windows
{
    public class Confirm : PopupWindow
    {
        private static int textLength = 46;

        private Button okBtn;
        private Button cancelBtn;

        public bool Result { get; private set; }

        public Confirm(Window parentWindow, string message, string Title = "Confirm")
            : base(Title, 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((double)message.Count() / textLength)), parentWindow)
        {
            this.Create(message, parentWindow);
        }

        public Confirm(string message, Window parentWindow, ConsoleColor backgroundColour, string Title = "message")
            : base(Title, 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((double)message.Count() / textLength)), parentWindow)
        {
            this.BackgroundColour = backgroundColour;
            this.Create(message, parentWindow);
        }

        private void Create(string message, Window parentWindow)
        {
            int count = 0;
            while ((count * 45) < message.Count())
            {
                string splitMessage = message.PadRight(textLength * (count + 1), ' ').Substring((count * textLength), textLength);
                Label messageLabel = new Label(splitMessage, this.PostionX + 2 + count, this.PostionY + 2, "messageLabel", this);
                this.Inputs.Add(messageLabel);
                count++;
            }

            this.okBtn = new Button(this.PostionX + this.Height - 2, this.PostionY + 2, "OK", "OkBtn", this)
            {
                Action = () =>
                {
                    this.Result = true;
                    this.ExitWindow();
                }
            };

            this.cancelBtn = new Button(this.PostionX + this.Height - 2, this.PostionY + 8, "Cancel", "cancelBtn", this)
            {
                Action = () => this.ExitWindow()
            };

            this.Inputs.Add(this.okBtn);
            this.Inputs.Add(this.cancelBtn);

            this.CurrentlySelected = this.okBtn;

            this.Draw();
            this.MainLoop();
        }
    }
}
