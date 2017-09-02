using SharpDX.DirectInput;
using System;
using System.Diagnostics;
using System.Linq;
using Common.Logging;
using Dartboard.Utils;

namespace DART.Dartboard.HID
{
    public class JoystickWrapper : LoggableObject
    {
        private Joystick _joystick;
        private readonly DirectInput _input;

        public JoystickWrapper()
        {
            _input = new DirectInput();

            // Find a Joystick Guid
            var joystickGuid = Guid.Empty;

            var devs = FindDevices(DeviceType.Joystick);
            var found = devs.FirstOrDefault();
            if (found != null)
                joystickGuid = found.InstanceGuid;

            if (joystickGuid == Guid.Empty)
            {
                Log.Error("No joystick/Gamepad found!");
            }
            else
            {
                Log.InfoFormat("Attempting to acquire Joystick with GUID {0}", joystickGuid);
                Acquire(joystickGuid);
                Acquired = true;
            }
        }

        private DeviceInstance[] FindDevices(DeviceType dt)
        {
            return _input.GetDevices(dt, DeviceEnumerationFlags.AllDevices).ToArray();
        }

        public bool Acquired { get; } = false;

        public DeviceInstance CurrentDeviceInfo => _joystick.Information;

        public void Acquire(Guid deviceGuid)
        {
            _joystick?.Dispose();
            _joystick = new Joystick(_input, deviceGuid);
            _joystick.Acquire();
            Log.InfoFormat("Acquired {0} with GUID {1}", _joystick.Information.ProductName, deviceGuid);
        }


        public JoystickState GetState()
        {
            _joystick?.Poll();
            return _joystick?.GetCurrentState();
        }
    }
}
