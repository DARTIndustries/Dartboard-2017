using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Dartboard.Utils;
using DART.Dartboard.Control.GenericRobot;
using DART.Dartboard.HID;
using DART.Dartboard.Models;
using DART.Dartboard.Models.HID;
using Vector3D;

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
            double x = joystick.X * joystick.Slider;
            double y = joystick.Y * joystick.Slider;

            double z = 0;
            if (joystick.Buttons[3])
                z -= 1;
            if (joystick.Buttons[4])
                z += 1;

            z *= joystick.Slider;

            var robotVector = new Vector(x, y, z);

            return new Do()
            {
                Lights = "",
                Motor = _robot.CalculateMotorValues(robotVector, 0,  gamepad)
            };
        }
    }
}