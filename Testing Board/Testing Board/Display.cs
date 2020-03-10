using System;
using CTRE.Gadgeteer.Module;
using Microsoft.SPOT;

namespace Testing_Board
{
    class Display
    {
        private readonly DisplayModule display;
        private readonly Font headerFont;
        private readonly Font bodyFont;
        private readonly DisplayModule.LabelSprite title;
        private readonly DisplayModule.LabelSprite[] lines;
        private readonly DisplayModule.ResourceImageSprite target;
        private const int targetHeightMultiplier = 20;

        public Display()
        {
            display = new DisplayModule(CTRE.HERO.IO.Port1, DisplayModule.OrientationType.Portrait);
            headerFont = Properties.Resources.GetFont(Properties.Resources.FontResources.VerdanaBold18);
            bodyFont = Properties.Resources.GetFont(Properties.Resources.FontResources.VerdanaReg14);
            display.SetBackgroundColor(DisplayModule.Color.Black);
            title = display.AddLabelSprite(headerFont, DisplayModule.Color.Green, 5, 0, 120, 24);
            lines = new DisplayModule.LabelSprite[] {
                display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 25, 120, 18),
                display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 45, 100, 18),
                display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 65, 120, 18),
                display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 85, 100, 18),
                display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 105, 120, 18),
                display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 125, 100, 18),
            };
            target = display.AddResourceImageSprite(
                Testing_Board.Properties.Resources.ResourceManager,
                Testing_Board.Properties.Resources.BinaryResources.ch2,
                Bitmap.BitmapImageType.Jpeg, 120, 26);
        }

        /*
         * Displays the main selecting menu
         */
        public void DisplayMainMenu()
        {
            title.SetText("Test Board");
            lines[0].SetText("Quadrature");
            lines[1].SetText("Analog");
            lines[2].SetText("Limit Switch");
            lines[3].SetText("Pigeon");
            lines[4].SetText("Brushed Ctrl");
            lines[5].SetText("Brushless Ctrl");
        }

        /*
         * Updates the selector
         */
        public void UpdateSelector(int state)
        {
            target.ForceRefresh();
            target.SetPosition(110, 26 + targetHeightMultiplier * state);
        }

        /*
         * Displays the text for each mode
         * 
         * @param mode the current mode
         * @param data array of data to be printed or checked
         */
        public void DisplayModes(int mode, string[] data)
        {
            target.Erase();
            switch (mode)
            {
                // Quadrature
                case 0:
                    {
                        title.SetText("Quadrature");
                        lines[2].SetText("Encoder Value:");
                        lines[3].SetText(data[0]);
                        break;
                    }

                // Analog
                case 1:
                    {
                        title.SetText("Analog");
                        lines[2].SetText("Analog Value:");
                        lines[3].SetText(data[0]);
                        break;
                    }

                // Limit Switch
                case 2:
                    {
                        title.SetText("Limit Switch");
                        lines[0].SetColor(DisplayModule.Color.Green);
                        lines[0].SetText("(If Closed, 1)");
                        lines[1].SetText("(LED=Closed)");
                        lines[2].SetText("Forward Value:");
                        lines[3].SetText(data[0]);
                        lines[4].SetText("Reverse Value:");
                        lines[5].SetText(data[1]);
                        break;
                    }

                // Pigeon
                case 3:
                    {
                        title.SetText("Pigeon");
                        lines[0].SetColor(DisplayModule.Color.Green);
                        lines[0].SetText("(Data Cable)");
                        lines[2].SetText("Pigeon Heading");
                        lines[3].SetText(data[0]);
                        break;
                    }

                // Brushed Motor Control
                case 4:
                    {
                        title.SetText("Motor Ctrl");
                        lines[0].SetText("Motor 2 Output:");
                        lines[2].SetText("Motor 1 Output:");
                        lines[4].SetText("Motor 0 Output:");

                        if (data[0] == "True")
                        {
                            lines[1].SetText(data[3]);
                            lines[3].SetText(data[2]);
                            lines[5].SetText(data[1]);
                        }
                        else
                        {
                            lines[1].SetText("OFF");
                            lines[3].SetText("OFF");
                            lines[5].SetText("OFF");
                        }
                        break;
                    }

                // Brushless Motor Control
                case 5:
                    {
                        title.SetText("Brushless");
                        lines[0].SetColor(DisplayModule.Color.Green);
                        lines[0].SetFont(headerFont);
                        lines[0].SetText("Control");
                        lines[2].SetText("Output:");
                        if (data[0] == "True")
                        {
                            lines[3].SetText(data[1]);
                        }
                        else
                        {
                            lines[3].SetText("OFF");
                        }
                        break;
                    }
            }
        }

        /*
         * Safety warning when the motor switch is on and entering the motor modes
         */
        public void Warning()
        {
            System.Threading.Thread.Sleep(100);
            title.SetColor(DisplayModule.Color.Red);
            title.SetText("WARNING");
            lines[1].SetText("Switch is ON");
            lines[2].SetText("Motors may move");
            lines[4].SetText("Message will clear");
            for (int i = 5; i > 0; i--)
            {
                lines[5].SetText("in " + i + " seconds.");
                System.Threading.Thread.Sleep(1000);
            }
        }

        /*
         * Clears the screen to input new text
         */
        public void Clear()
        {
            title.SetColor(DisplayModule.Color.Green);
            title.SetText("");
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].SetColor(DisplayModule.Color.White);
                lines[i].SetText("");
            }
            lines[0].SetFont(bodyFont);
            target.Erase();
        }
    }
}
