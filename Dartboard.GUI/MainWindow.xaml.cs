using System;
using System.Timers;
using System.Windows;
using Common.Logging;
using DART.Dartboard.Control;
using DART.Dartboard.Control.Dart2017;
using DART.Dartboard.GUI.Logging;
using MjpegProcessor;
using DART.Dartboard.HID;
using Simulator.Control3D;

namespace DART.Dartboard.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer t;

        private ILog _log = LogManager.GetLogger<MainWindow>();

        private InputProcessor _proc;

        public MainWindow()
        {
            InitializeComponent();
            ConsoleOutput.Text = "";
            ConsoleOutput.ScrollToEnd();
            TextBoxLogger.GlobalLogTextBox = ConsoleOutput;

            HIDManager.SharedManager.AcquireAll();

            var robot = Robot.LoadFromFile(@".\Robots\DartV1\robot.json");

            virtualRobot.LoadRobot(robot);

            _proc = new InputProcessor(new Robot2017());

            GlobalPulse.Pulse += GlobalPulseOnPulse;
        }

        private void GlobalPulseOnPulse(TimeSpan timeSpan)
        {
            var joy = HidDisplay.JoystickState = HIDManager.SharedManager.GetJoystickState();
            var game = HidDisplay.GamepadState = HIDManager.SharedManager.GetGamepadState();

            var _do = _proc.Process(timeSpan, game, joy);

            for (var i = 0; i < _do.Motor.Length; i++)
            {
                virtualRobot.Robot.MotorContoller[i].Thrust = _do.Motor[i];
            }

            Dispatcher.Invoke(() =>
            {
                virtualRobot.Tick(timeSpan);
            });
        }
    }
}
