using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using CTRE.Gadgeteer.Module;

namespace Testing_Board
{
    public class Program
    {
        static AnalogInput motorDial = new AnalogInput(CTRE.HERO.IO.Port1.Analog_Pin3);
        static AnalogInput leftButton = new AnalogInput(CTRE.HERO.IO.Port1.Analog_Pin4);
        static AnalogInput rightButton = new AnalogInput(CTRE.HERO.IO.Port1.Analog_Pin5);
        static DisplayModule display = new DisplayModule(CTRE.HERO.IO.Port1, DisplayModule.OrientationType.Portrait);
        
        static Font headerFont = Properties.Resources.GetFont(Properties.Resources.FontResources.VerdanaBold18);
        static Font bodyFont = Properties.Resources.GetFont(Properties.Resources.FontResources.VerdanaReg14);

        DisplayModule.LabelSprite title, value;

        public void displaySetup()
        {
            title = display.AddLabelSprite(headerFont, DisplayModule.Color.Green, 40, 0, 80, 16);
            value = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 30, 50, 100, 15);
        }
        public static void Main()
        {
            int state = 0;
            /* loop forever */
            while (true)
            {
                switch(state){
                    default: {
                        // Display Welcome
                        break;
                    }
                }

                /* wait a bit */
                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
