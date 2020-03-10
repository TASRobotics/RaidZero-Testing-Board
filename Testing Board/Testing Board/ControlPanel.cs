using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Testing_Board
{
    class ControlPanel
    {
        private readonly AnalogInput[] sliders;
        private readonly InputPort[] io;
        private readonly bool[] prev;

        public ControlPanel()
        {
            sliders = new AnalogInput[] {
                new AnalogInput(CTRE.HERO.IO.Port8.Analog_Pin3),
                new AnalogInput(CTRE.HERO.IO.Port8.Analog_Pin4),
                new AnalogInput(CTRE.HERO.IO.Port8.Analog_Pin5)
            };

            io = new InputPort[]
            {
                new InputPort(CTRE.HERO.IO.Port7.Pin3, false, Port.ResistorMode.PullDown),
                new InputPort(CTRE.HERO.IO.Port7.Pin6, false, Port.ResistorMode.PullDown),
                new InputPort(CTRE.HERO.IO.Port7.Pin5, false, Port.ResistorMode.PullDown),
                new InputPort(CTRE.HERO.IO.Port7.Pin9, false, Port.ResistorMode.PullDown),
                new InputPort(CTRE.HERO.IO.Port7.Pin8, false, Port.ResistorMode.PullDown),
            };

            prev = new bool[4];
        }

        /*
         * Gets the slider value
         * 
         * @return the value of the slider from -1.0 to 1.0
         */
        public double GetSlider(int num)
        {
            return ((1 - sliders[num].Read()) * 2 - 1) * System.Math.Abs((1 - sliders[num].Read()) * 2 - 1);
        }

        /*
         * Gets up button when it is pressed
         * 
         * @return true if up button is pressed
         */
        public bool GetUpButtonPressed()
        {
            return io[0].Read() && !prev[0];
        }

        /*
         * Gets down button when it is pressed
         * 
         * @return true if down button is pressed
         */
        public bool GetDownButtonPressed()
        {
            return io[1].Read() && !prev[1];
        }

        /*
         * Gets green button when it is pressed
         * 
         * @return true if green button is pressed
         */
        public bool GetGreenButtonPressed()
        {
            return io[2].Read() && !prev[2];
        }

        /*
         * Gets red button when it is pressed
         * 
         * @return true if red button is pressed
         */
        public bool GetRedButtonPressed()
        {
            return io[3].Read() && !prev[3];
        }

        /*
         * Get if the motor safety switch is on or not
         * 
         * @return true if switch is on
         */
        public bool GetMotorSafetySwitch()
        {
            return io[4].Read();
        }

        /*
         * Update the buttons for toggle check
         */
        public void UpdateToggle()
        {
            for (int i = 0; i < prev.Length; i++)
            {
                prev[i] = io[i].Read();
            }
        }
    }
}
