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

        public JoystickWrapper(DeviceType type)
        {
            _input = new DirectInput();

            // Find a Joystick Guid
            var joystickGuid = Guid.Empty;

            var devs = FindDevices(type);
            var found = devs.FirstOrDefault();
            if (found != null)
                joystickGuid = found.InstanceGuid;

            if (joystickGuid == Guid.Empty)
            {
                Log.Error("No joystick/Gamepad found!");
            }
            else
            {
                Log.InfoFormat("Attempting to acquire {0} with GUID {1}", type, joystickGuid);
                Acquire(joystickGuid);
                Acquired = true;
            }
        }

        public DeviceInstance[] FindDevices(DeviceType dt)
        {
            var d = (SharpDX.DirectInput.DeviceType) dt;
            return _input.GetDevices(d, DeviceEnumerationFlags.AllDevices).ToArray();
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

        public JoystickState _currentState;

        public void Poll()
        {
            _joystick.Poll();
            if (_joystick == null)
                return;

            var s = _joystick.GetCurrentState();
            _currentState = new JoystickState();
            _currentState.X = s.X;
        }
    }
}
