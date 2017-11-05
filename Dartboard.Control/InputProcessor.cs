using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DART.Dartboard.Control.GenericRobot;
using DART.Dartboard.HID;
using DART.Dartboard.Models;
using DART.Dartboard.Models.HID;
using DART.Dartboard.Utils;
using MathNet.Numerics.LinearAlgebra;

namespace DART.Dartboard.Control
{
    public class InputProcessor : LoggableObject
    {
        private readonly Robot _robot;

        public InputProcessor(Robot robot)
        {
            _robot = robot;
        }

        public Do Process(TimeSpan timeSince, GamepadState gamepad, JoystickState joystick)
        {
            var @do = new Do();
            @do.Lights = "";

            if (joystick != null)
            {
                var sliderLevel = joystick.Slider;

                if (joystick.Buttons[2]) // Overdrive
                {
                    sliderLevel *= 2.0;
                }

                double x = joystick.X * sliderLevel;
                double y = joystick.Y * sliderLevel;

                double z = 0;
                if (joystick.Buttons[3] == joystick.Buttons[4])
                    z = 0;
                else if (joystick.Buttons[4])
                    z = 1;
                else if (joystick.Buttons[3])
                    z = -1;

                z *= sliderLevel;

                Log.InfoFormat("X: {0:F2}, Y: {1:F2}, Z: {2:F2}", x, y, z);

                var robotVector = Vector<double>.Build.DenseOfArray(new[] {x, y, z});

                @do.Motor = _robot.CalculateMotorValues(robotVector, joystick.RotationZ, gamepad);
            }

            if (gamepad != null)
            {
                
            }

            return @do;
        }
    }
}