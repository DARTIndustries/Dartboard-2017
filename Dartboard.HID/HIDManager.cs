using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Logging;
using Dartboard.Utils;
using DART.Dartboard.Models.HID;
using SharpDX.XInput;
using static Dartboard.Utils.Numerics;

namespace DART.Dartboard.HID
{
    public sealed class HIDManager : LoggableObject
    {
        public const int JoystickPrecision = 3;

        static HIDManager()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SharpDX.DirectInput.JoystickState, JoystickState>()
                .ConvertUsing((state, y) =>
                    {
                        if (y == null)
                            y = new JoystickState();

                        y.X = JoystickFunction(state.X, 65535, 0);
                        y.Y = JoystickFunction(state.Y, 65535, 0);
                        y.RotationZ = JoystickFunction(state.RotationZ, 65535, 0);
                        y.Slider = TriggerFunction(state.Sliders[0], 65535, 0);

                        y.Buttons = (new[] {false}).Concat(state.Buttons.Take(12)).ToArray();
                        y.HatSwitch = (HatPosition) state.PointOfViewControllers[0];

                        return y;
                    });

                cfg.CreateMap<SharpDX.XInput.Gamepad, GamepadState>()
                .ConvertUsing((state, ret) =>
                    {
                        if (ret == null)
                            ret = new GamepadState();

                        ret.Buttons = (Models.HID.GamepadButtonFlags)state.Buttons;

                        ret.LeftThumbX = SignedJoystickFunction(state.LeftThumbX);
                        ret.LeftThumbY = SignedJoystickFunction(state.LeftThumbY);

                        ret.RightThumbX = SignedJoystickFunction(state.RightThumbX);
                        ret.RightThumbY = SignedJoystickFunction(state.RightThumbY);

                        ret.LeftTrigger = TriggerFunction(state.LeftTrigger, 0, 255);
                        ret.RightTrigger = TriggerFunction(state.RightTrigger, 0, 255);

                        return ret;

                    });
            });
        }

        private static HIDManager _sharedManager;
        public static HIDManager SharedManager
        {
            get => _sharedManager ?? (_sharedManager = new HIDManager());
            set => _sharedManager = value;
        }

        private readonly HIDConfig _config;

        public HIDManager()
        {
            _config = new HIDConfig()
            {
                ControllerDeadZone = 0.25,
                JoystickDeadZone = 0.10,
                FailOnNoDeviceFound = false
            };
        }

        public HIDManager(HIDConfig config)
        {
            _config = config;
        }

        #region Joystick

        private JoystickWrapper _wrapper;
        public JoystickState GetJoystickState()
        {
            if (_wrapper == null)
                _wrapper = new JoystickWrapper();

            if (!_wrapper.Acquired && _config.FailOnNoDeviceFound)
            {
                Log.Fatal("Unable to connect to a joystick");
                throw new UnableToAcquireDeviceException("Joystick");
            }

            var js = Mapper.Map<JoystickState>(_wrapper.GetState());
            if (_config.JoystickDeadZone.HasValue)
                js.ApplyDeadZone(_config.JoystickDeadZone.Value);

            js.RoundAll(JoystickPrecision);
            return js;
        }

        public string JoystickInfo => _wrapper.CurrentDeviceInfo.ProductName;

        #endregion

        #region Gamepad

        private SharpDX.XInput.Controller _controller;
        public GamepadState GetGamepadState()
        {
            if (_controller == null || !_controller.IsConnected)
            {
                _controller = AcquireController();
                if (_controller == null)
                {
                    Log.Fatal("Unable to connect to controller");
                    throw new UnableToAcquireDeviceException("Controller");
                }
            }

            var state = _controller.GetState();

            var gs = Mapper.Map<GamepadState>(state.Gamepad);

            if (_config.ControllerDeadZone.HasValue)
                gs.ApplyDeadZone(_config.ControllerDeadZone.Value);

            gs.RoundAll(JoystickPrecision);
            return gs;
        }

        private Controller AcquireController()
        {
            for (int i = 0; i < 4; i++)
            {
                var controller = new Controller((UserIndex)i);
                if (controller.IsConnected)
                    return controller;
            }
            return null;
        }

        #endregion

    }
}
