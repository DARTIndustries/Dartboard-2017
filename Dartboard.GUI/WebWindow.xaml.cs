using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Media.Media3D;
using CefSharp;
using Common.Logging;
using DART.Dartboard.Control;
using DART.Dartboard.Control.GenericRobot;
using DART.Dartboard.GUI.Logging;
using DART.Dartboard.HID;
using DART.Dartboard.Models.Configuration;
using DART.Dartboard.Networking;
using DART.Dartboard.Utils;
using Newtonsoft.Json;
using Robot = Simulator.Control3D.Robot;
using VectorConverter = DART.Dartboard.Utils.VectorConverter;

namespace DART.Dartboard.GUI
{
    /// <summary>
    /// Interaction logic for WebWindow.xaml
    /// </summary>
    public partial class WebWindow : Window
    {
        private Timer t;

        private ILog _log = LogManager.GetLogger<WebWindow>();

        private InputProcessor _proc;

        public WebWindow()
        {
            InitializeComponent();
            ConsoleOutput.Text = "";
            ConsoleOutput.ScrollToEnd();
            TextBoxLogger.GlobalLogTextBox = ConsoleOutput;

            HIDManager.SharedManager.AcquireAll();


            var path = @"C:\Users\DART\Source\Repos\Dartboard\Simulator\Robots\DartV2\robot.json";
            var _3DRobot = Robot.LoadFromFile(path);
            var robot = JsonConvert.DeserializeObject<RobotConfiguration>(File.ReadAllText(path), new VectorConverter(), new MatrixConverter());

            //_3DRobot.Model.Transform = new MatrixTransform3D(new Matrix3D())

            virtualRobot.LoadRobot(_3DRobot, robot);

            _proc = new InputProcessor(new JsonRobot(robot));

            var uri = "file://" + 
                Path.GetFullPath(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Web", "index.html"));

            browser.Address = uri.Replace('\\', '/');

            _log.Info(uri);

            _interface = new TcpNetworkInterface(new JsonMessageFormatter(), new Uri("tcp://10.0.0.2:5000"));
           // _interface = new TcpNetworkInterface(new JsonMessageFormatter(), new Uri("tcp://129.25.218.183:5000"));
            //_interface = new TcpNetworkInterface(new JsonMessageFormatter(), new Uri("tcp://10.250.29.35:5000"));

            GlobalPulse.Pulse += GlobalPulseOnPulse;
        }

        private WatchGroup _buttonGroup = new WatchGroup();
        private INetworkInterface _interface;

        private void GlobalPulseOnPulse(TimeSpan timeSpan)
        {

            var joy = HidDisplay.JoystickState = HIDManager.SharedManager.GetJoystickState();
            var game = HidDisplay.GamepadState = HIDManager.SharedManager.GetGamepadState();

            var _do = _proc.Process(timeSpan, game, joy);

            for (var i = 0; i < _do.Motor.Length; i++)
            {
                virtualRobot.Robot.MotorContoller[i].Thrust = _do.Motor[i];
            }

            for (var i = 0; i < joy.Buttons.Length; i++)
            {
                _buttonGroup[i].Update(joy.Buttons[i]);
            }

            //if (_buttonGroup[7].RisingEdge())
            //{
            //    browser.Reload();
            //}

            //if (_buttonGroup[8].RisingEdge() || joy.Buttons[9])
            //{
            //    _log.Warn(JsonConvert.SerializeObject(_do));
            //}
            try
            {
                Dispatcher.Invoke(() =>
                {
                    virtualRobot.Tick(timeSpan);
                });
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                //suppress
            }

            

            _interface?.Send(_do);
        }
    }
}
