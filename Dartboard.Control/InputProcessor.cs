using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DART.Dartboard.Control.GenericRobot;
using DART.Dartboard.HID;

namespace DART.Dartboard.Control
{
    public class InputProcessor
    {
        private readonly Robot _robot;

        public InputProcessor(Robot robot)
        {
            _robot = robot;
            HIDManager.SharedManager.AcquireAll();
            GlobalPulse.Pulse += PulseRecieve;
        }

        ~InputProcessor()
        {
            GlobalPulse.Pulse -= PulseRecieve;
        }

        private void PulseRecieve(TimeSpan timeSince)
        {
            // Gampad will be used for the co-pilot. It will be in charge of the servo arm
            var gamepad = HIDManager.SharedManager.GetGamepadState();

            // Joystick will be the for the pilot. It controls primary motor functions. 
            var joystick = HIDManager.SharedManager.GetJoystickState();

            
        }
    }
}