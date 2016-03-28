using ConsoleDraw.Inputs;
using ConsoleDraw.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDraw.Windows
{
    public class Alert : PopupWindow
    {
        private Button okBtn;
        private static int textLength = 46;


        public Alert(string Message, Window parentWindow)
            : base("Message", 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((double)Message.Count() / textLength)), parentWindow)
        {
            this.Create(Message, parentWindow);
        }

        public Alert(string Message, Window parentWindow, string Title)
            : base(Title, 6, (Console.WindowWidth / 2) - 30, 25, 5 + (int)Math.Ceiling(((double)Message.Count() / textLength)), parentWindow)
        {
            this.Create(Message, parentWindow);
        }

        public Alert(string Message, Window parentWindow, ConsoleColor backgroundColour)
            : base("Message", 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((double)Message.Count() / textLength)), parentWindow)
        {
            this.BackgroundColour = backgroundColour;

            this.Create(Message, parentWindow);
        }

        public Alert(string Message, Window parentWindow, ConsoleColor backgroundColour, string Title)
            : base(Title, 6, (Console.WindowWidth / 2) - 25, 50, 5 + (int)Math.Ceiling(((double)Message.Count() / textLength)), parentWindow)
        {
            this.BackgroundColour = backgroundColour;

            this.Create(Message, parentWindow);
        }

        private void Create(string Message, Window parentWindow)
        {
            int count = 0;
            while ((count*45) < Message.Count())
            {
                string splitMessage = Message.PadRight(textLength * (count + 1), ' ').Substring((count * textLength), textLength);
                Label messageLabel = new Label(splitMessage, this.PostionX + 2 + count, this.PostionY + 2, "messageLabel", this);
                this.Inputs.Add(messageLabel);

                count++;
            }

            /*
            var messageLabel = new Label(Message, PostionX + 2, PostionY + 2, "messageLabel", this);
            messageLabel.BackgroundColour = BackgroundColour;*/

            this.okBtn = new Button(this.PostionX + this.Height - 2, this.PostionY + 2, "OK", "OkBtn", this);
            this.okBtn.Action = delegate() { this.ExitWindow(); };

            this.Inputs.Add(this.okBtn);

            this.CurrentlySelected = this.okBtn;

            this.Draw();
            this.MainLoop();
        }
    }
}
