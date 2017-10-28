using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using Common.Logging;
using DART.Dartboard.Control;
using DART.Dartboard.Control.GenericRobot;
using DART.Dartboard.GUI.Logging;
using MjpegProcessor;
using DART.Dartboard.HID;
using DART.Dartboard.Models.Configuration;
using DART.Dartboard.Utils;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
using Robot = Simulator.Control3D.Robot;
using VectorConverter = DART.Dartboard.Utils.VectorConverter;

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


            var path = @".\Robots\DartV1\robot.json";
            var _3DRobot = Robot.LoadFromFile(path);
            var robot = JsonConvert.DeserializeObject<RobotConfiguration>(File.ReadAllText(path), new VectorConverter(), new MatrixConverter());

            virtualRobot.LoadRobot(_3DRobot);

            _proc = new InputProcessor(new JsonRobot(robot));

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
