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

            if (false && joystick != null)
            {
                var sliderLevel = joystick.Slider;
                
                if (joystick.Buttons[2]) // Overdrive
                {
                    sliderLevel *= 2.0;
                }

                if (joystick.Buttons[1]) // Direct Motor Testing
                {
                    sbyte level = joystick.Buttons[2] ? (sbyte)127 : (sbyte)-127;

                    @do.Motor = new sbyte[]
                    {
                        joystick.Buttons[7] ? level : (sbyte)0,
                        joystick.Buttons[8] ? level : (sbyte)0,
                        joystick.Buttons[9] ? level : (sbyte)0,
                        joystick.Buttons[10] ? level : (sbyte)0,
                        joystick.Buttons[11] ? level : (sbyte)0,
                        joystick.Buttons[12] ? level : (sbyte)0,
                    };
                    return @do;
                }

                if (joystick.Buttons[1])
                {
                    var level = (sbyte)(Math.Abs(joystick.X * sliderLevel) * 127);

                    @do.Motor = new sbyte[]
                    {
                        0,
                        0,
                        joystick.X >= 0 ? level : (sbyte)-level,
                        joystick.X <= 0 ? level : (sbyte)-level,
                        0,
                        0
                    };

                    return @do;
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

                var robotVector = Vector<double>.Build.DenseOfArray(new[] {x, y, z});

                double rot = 0;
                if ((gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0)
                    rot -= 1;
                if ((gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0)
                    rot += 1;


                @do.Motor = _robot.CalculateMotorValues(robotVector, 0, gamepad);


                var horiz = (short)(((gamepad.RightThumbX + 1) / 2.0) * 180.0);
                var vert = (short)(((gamepad.RightThumbY + 1) / 2.0) * 180.0);


                @do.Camera = new Camera
                {
                    Angles = new[] { horiz, vert }
                };

                @do.Motor[0] *= -1;
                @do.Motor[1] *= -1;
                @do.Motor[2] *= -1;
                @do.Motor[3] *= -1;
                @do.Motor[4] *= -1;
                @do.Motor[5] *= -1;
            }

            if (gamepad != null)
            {
                double x = gamepad.LeftThumbX;
                double y = gamepad.LeftThumbY;

                double z = -gamepad.LeftTrigger + gamepad.RightTrigger;

                var horiz = (short)(((gamepad.RightThumbX + 1) / 2.0) * 180.0);
                var vert = (short)(((gamepad.RightThumbY + 1) / 2.0) * 180.0);

                @do.Camera = new Camera
                {
                    Angles = new[] { horiz, vert }
                };

                z = -z;

                var robotVector = Vector<double>.Build.DenseOfArray(new[] { x, y, z });

                @do.Motor = _robot.CalculateMotorValues(robotVector, joystick.RotationZ, gamepad);
            }

            return @do;
        }
    }
}