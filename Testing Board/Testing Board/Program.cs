using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using CTRE.Gadgeteer.Module;
using CTRE.Phoenix.MotorControl.CAN;
using CTRE.Phoenix.MotorControl;

namespace Testing_Board
{
    public class Program
    {
        static TalonSRX rightTal = new TalonSRX(0);
        static TalonSRX leftTal = new TalonSRX(1);

        static AnalogInput motorDial = new AnalogInput(CTRE.HERO.IO.Port8.Analog_Pin3);
        static InputPort leftButton = new InputPort(CTRE.HERO.IO.Port8.Pin4, false, Port.ResistorMode.PullDown);
        static InputPort rightButton = new InputPort(CTRE.HERO.IO.Port8.Pin5, false, Port.ResistorMode.PullDown);
        static DisplayModule display = new DisplayModule(CTRE.HERO.IO.Port1, DisplayModule.OrientationType.Portrait);

        static Font headerFont = Properties.Resources.GetFont(Properties.Resources.FontResources.VerdanaBold18);
        static Font bodyFont = Properties.Resources.GetFont(Properties.Resources.FontResources.VerdanaReg14);

        static DisplayModule.LabelSprite title, value1, value2, tal1, tal2;

        public static void DisplaySetup()
        {
            title = display.AddLabelSprite(headerFont, DisplayModule.Color.Green, 5, 0, 120, 24);
            tal1 = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 40, 120, 16);
            value1 = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 15, 60, 120, 16);
            tal2 = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 80, 120, 16);
            value2 = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 15, 100, 120, 16);
        }

        public static void StartUpSetup()
        {
            title.SetText("Test Board");
            tal1.SetText("Press Green Button");
            value1.SetPosition(5, 60);
            value1.SetText("to Start");
        }

        public static void MainSetup()
        {
            value1.SetPosition(15, 60);
        }

        public static void Main()
        {
            DisplaySetup();
            bool start = false;
            bool rightButtonPrev = false;
            bool leftButtonPrev = false;
            int state = -1;
            /* loop forever */
            while (true)
            {
                rightButtonPrev = rightButton.Read();
                if ((rightButton.Read() || leftButton.Read()) && !start)
                {
                    start = true;
                    MainSetup();
                    state = 0;
                }
                else if (rightButton.Read() && !rightButtonPrev && start)
                {
                    state++;
                }
                else if (leftButton.Read() && !leftButtonPrev && start)
                {
                    state--;
                }

                if (state >= 4)
                {
                    state = 0;
                }
                if (state <= -1)
                {
                    state = 3;
                }

                if (start)
                {
                    switch (state)
                    {
                        case 0: // Quadrature
                            {
                                leftTal.ConfigSelectedFeedbackSensor(FeedbackDevice.QuadEncoder);
                                rightTal.ConfigSelectedFeedbackSensor(FeedbackDevice.QuadEncoder);
                                title.SetText("Quadrature");
                                tal1.SetText("LTalon Value:");
                                value1.SetText(leftTal.GetSelectedSensorPosition().ToString());
                                tal1.SetText("RTalon Value:");
                                value2.SetText(rightTal.GetSelectedSensorPosition().ToString());
                                break;
                            }

                        case 1: // Analog
                            {
                                leftTal.ConfigSelectedFeedbackSensor(FeedbackDevice.Analog);
                                rightTal.ConfigSelectedFeedbackSensor(FeedbackDevice.Analog);
                                title.SetText("Analog");
                                tal1.SetText("LTalon Value:");
                                value1.SetText(leftTal.GetSelectedSensorPosition().ToString());
                                tal1.SetText("RTalon Value:");
                                value2.SetText(rightTal.GetSelectedSensorPosition().ToString());
                                break;
                            }

                        case 2: // Limit Switch (only on left tal)
                            {
                                leftTal.ConfigForwardLimitSwitchSource(LimitSwitchSource.FeedbackConnector, LimitSwitchNormal.NormallyOpen);
                                leftTal.ConfigReverseLimitSwitchSource(LimitSwitchSource.FeedbackConnector, LimitSwitchNormal.NormallyOpen);
                                title.SetText("Limit Switch");
                                tal1.SetText("Forward Value:");
                                value1.SetText("false");
                                tal1.SetText("Reverse Value:");
                                value2.SetText("false");
                                break;
                            }

                        case 3: // Motor Contorl
                            {
                                title.SetText("Motor Ctrl");
                                tal1.SetText("LMotor Value:");
                                value1.SetText(motorDial.Read().ToString());
                                tal1.SetText("RMotor Value:");
                                value2.SetText(motorDial.Read().ToString());
                                break;
                            }

                        default: // StartUp Page
                            {
                                StartUpSetup();
                                break;
                            }
                    }
                }
                else
                {
                    StartUpSetup();
                }
                
                /* wait a bit */
                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
