using Microsoft.SPOT;
using CTRE.Gadgeteer.Module;
using CTRE.Phoenix.MotorControl.CAN;
using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.Sensors;
using System;

namespace Testing_Board
{
    public class Program
    {
        private const int NUMBER_OF_STATE = 6;

        private static TalonSRX motor0;
        private static VictorSPX motor1;
        private static VictorSPX motor2;
        private static PWMSpeedController brushlessOutput;

        private static ControlPanel control;
        private static Display display;


        /*
         * Initialize the system
         */
        private static void Init()
        {
            // Initialize Display
            control = new ControlPanel();
            display = new Display();

            motor0 = new TalonSRX(0);
            motor1 = new VictorSPX(0);
            motor2 = new VictorSPX(1);
            brushlessOutput = new PWMSpeedController(CTRE.HERO.IO.Port3.PWM_Pin4);

            // Reset Motor Controllers
            motor0.ConfigFactoryDefault();
            motor1.ConfigFactoryDefault();
            motor2.ConfigFactoryDefault();
            brushlessOutput.Enable();
            display.Clear();
        }

        public static void Main()
        {
            Init();
            bool inMenu = true;
            int state = 0;

            while (true)
            {
                if (inMenu)
                {
                    // Buttons for selecting the modes
                    if (control.GetDownButtonPressed())
                    {
                        state++;
                        if (state >= NUMBER_OF_STATE)
                        {
                            state = 0;
                        }
                    }
                    else if (control.GetUpButtonPressed())
                    {
                        state--;
                        if (state <= -1)
                        {
                            state = NUMBER_OF_STATE - 1;
                        }
                    }

                    // Display Menu and selector
                    display.DisplayMainMenu();
                    display.UpdateSelector(state);
                }
                else
                {
                    switch (state)
                    {
                        // Quadrature
                        case 0: 
                            {
                                motor0.ConfigSelectedFeedbackSensor(FeedbackDevice.QuadEncoder);
                                display.DisplayModes(state, new string[] { motor0.GetSelectedSensorPosition().ToString() });
                                break;
                            }

                        // Analog
                        case 1: 
                            {
                                motor0.ConfigSelectedFeedbackSensor(FeedbackDevice.Analog);
                                display.DisplayModes(state, new string[] { motor0.GetSelectedSensorPosition().ToString() });
                                break;
                            }

                        // Limit Switch
                        case 2: 
                            {
                                int forward;
                                int reverse;
                                motor0.GetSensorCollection().IsFwdLimitSwitchClosed(out forward);
                                motor0.GetSensorCollection().IsRevLimitSwitchClosed(out reverse);
                                display.DisplayModes(state, new string[] { forward.ToString(), reverse.ToString() });
                                break;
                            }

                        // Pigeon
                        case 3: 
                            {
                                PigeonIMU pidgey = new PigeonIMU(motor0);
                                display.DisplayModes(state, new string[] { pidgey.GetCompassHeading().ToString() });
                                break;
                            }

                        // Brushed Motor Control
                        case 4: 
                            {
                                display.DisplayModes(state, new string[] {
                                        control.GetMotorSafetySwitch().ToString(),
                                        control.GetSlider(0).ToString(),
                                        control.GetSlider(1).ToString(),
                                        control.GetSlider(2).ToString()
                                        });
                                if (control.GetMotorSafetySwitch())
                                {
                                    motor0.Set(ControlMode.PercentOutput, control.GetSlider(0));
                                    motor1.Set(ControlMode.PercentOutput, control.GetSlider(1));
                                    motor2.Set(ControlMode.PercentOutput, control.GetSlider(2));
                                }
                                else
                                {
                                    motor0.Set(ControlMode.PercentOutput, 0);
                                    motor1.Set(ControlMode.PercentOutput, 0);
                                    motor2.Set(ControlMode.PercentOutput, 0);
                                }
                                break;
                            }

                        // Brushless Motor Control
                        case 5:
                            {
                                display.DisplayModes(state, new string[] {
                                        control.GetMotorSafetySwitch().ToString(),
                                        control.GetSlider(0).ToString(),
                                        });
                                if (control.GetMotorSafetySwitch())
                                {
                                    brushlessOutput.Enable();
                                    brushlessOutput.Set((float)control.GetSlider(0));
                                }
                                else
                                {
                                    brushlessOutput.Disable();
                                }
                                break;
                            }
                    }
                    CTRE.Phoenix.Watchdog.Feed();
                }

                // Buttons for entering desired mode
                if (control.GetRedButtonPressed())
                {
                    inMenu = true;
                    display.Clear();
                }
                else if (control.GetGreenButtonPressed())
                {
                    inMenu = false;
                    display.Clear();
                    if ((state == 4 || state == 5) && control.GetMotorSafetySwitch())
                    {
                        display.Warning();
                        display.Clear();
                    }
                }

                control.UpdateToggle();

                /* wait a bit */
                System.Threading.Thread.Sleep(40);
            }
        }
    }
}
