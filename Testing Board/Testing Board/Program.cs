using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using CTRE.Gadgeteer.Module;
using CTRE.Phoenix.MotorControl.CAN;
using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.Sensors;

namespace Testing_Board
{
    public class Program
    {
        static readonly int NUMBER_OF_STATE = 5;

        static TalonSRX rightTal = new TalonSRX(0);
        static TalonSRX leftTal = new TalonSRX(1);

        static AnalogInput motorDial = new AnalogInput(CTRE.HERO.IO.Port8.Analog_Pin3);
        static InputPort leftButton = new InputPort(CTRE.HERO.IO.Port8.Pin4, false, Port.ResistorMode.PullDown);
        static InputPort rightButton = new InputPort(CTRE.HERO.IO.Port8.Pin5, false, Port.ResistorMode.PullDown);
        static InputPort dialSwitch = new InputPort(CTRE.HERO.IO.Port8.Pin6, false, Port.ResistorMode.PullDown);
        static DisplayModule display = new DisplayModule(CTRE.HERO.IO.Port1, DisplayModule.OrientationType.Portrait);

        static Font headerFont = Properties.Resources.GetFont(Properties.Resources.FontResources.VerdanaBold18);
        static Font bodyFont = Properties.Resources.GetFont(Properties.Resources.FontResources.VerdanaReg14);

        static DisplayModule.LabelSprite title, value1, value2, tal1, tal2;

        public static void DisplaySetup()
        {
            title = display.AddLabelSprite(headerFont, DisplayModule.Color.Green, 5, 0, 120, 24);
            tal1 = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 40, 120, 18);
            value1 = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 15, 60, 100, 18);
            tal2 = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 5, 80, 120, 18);
            value2 = display.AddLabelSprite(bodyFont, DisplayModule.Color.White, 15, 100, 100, 18);
        }

        public static void StartUpSetup()
        {
            title.SetText("Test Board");
            tal1.SetText("Press Button");
            value1.SetText("to Start");
        }

        public static void MainSetup()
        {
            value1.SetText("");
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
                if ((rightButton.Read() || leftButton.Read()) && !start)
                {
                    start = true;
                    MainSetup();
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
                                tal2.SetText("RTalon Value:");
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
                                tal2.SetText("RTalon Value:");
                                value2.SetText(rightTal.GetSelectedSensorPosition().ToString());
                                break;
                            }

                        case 2: // Limit Switch (only on left tal)
                            {
                                leftTal.ConfigForwardLimitSwitchSource(LimitSwitchSource.FeedbackConnector, LimitSwitchNormal.NormallyOpen);
                                leftTal.ConfigReverseLimitSwitchSource(LimitSwitchSource.FeedbackConnector, LimitSwitchNormal.NormallyOpen);
                                title.SetText("Limit Switch");
                                tal1.SetText("Forward Value:");
                                int forward = -1;
                                int reverse = -1;
                                leftTal.GetSensorCollection().IsFwdLimitSwitchClosed(out forward);
                                rightTal.GetSensorCollection().IsRevLimitSwitchClosed(out reverse);
                                value1.SetText(forward.ToString());
                                tal2.SetText("Reverse Value:");
                                value2.SetText(reverse.ToString());
                                break;
                            }

                        case 3: // Motor Contorl
                            {
                                title.SetText("Motor Ctrl");
                                tal1.SetText("Motor Output:");
                                tal2.SetText("");
                                value2.SetText("");
                                if (dialSwitch.Read())
                                {
                                    double motorPower = (motorDial.Read() - 0.5) * 2;
                                    leftTal.Set(ControlMode.PercentOutput, motorPower);
                                    rightTal.Set(ControlMode.PercentOutput, motorPower);
                                    value1.SetText(motorPower.ToString());
                                }
                                else
                                {
                                    leftTal.Set(ControlMode.PercentOutput, 0);
                                    rightTal.Set(ControlMode.PercentOutput, 0);
                                    value1.SetText("OFF");
                                }
                                break;
                            }

                        case 4: // Pigeon
                            {
                                PigeonIMU leftPidgey = new PigeonIMU(leftTal);
                                PigeonIMU rightPidgey = new PigeonIMU(rightTal);
                                title.SetText("PigeonIMU");
                                tal1.SetText("LPigeon Value:");
                                value1.SetText(leftPidgey.GetCompassHeading().ToString());
                                tal2.SetText("RPigeon Value:");
                                value2.SetText(rightPidgey.GetCompassHeading().ToString());
                                break;
                            }

                        default: // StartUp Page
                            {
                                StartUpSetup();
                                break;
                            }
                    }
                    if (rightButton.Read() && !rightButtonPrev)
                    {
                        state++;
                    }
                    else if (leftButton.Read() && !leftButtonPrev)
                    {
                        state--;
                    }

                    if (state >= NUMBER_OF_STATE)
                    {
                        state = 0;
                    }
                    if (state <= -1)
                    {
                        state = NUMBER_OF_STATE - 1;
                    }
                }
                else
                {
                    StartUpSetup();
                }
                rightButtonPrev = rightButton.Read();
                leftButtonPrev = leftButton.Read();
                CTRE.Phoenix.Watchdog.Feed();
                /* wait a bit */
                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
