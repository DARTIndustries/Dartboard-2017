using System;
using System.Timers;
using System.Windows;
using Common.Logging;
using DART.Dartboard.GUI.Logging;
using MjpegProcessor;
using DART.Dartboard.HID;

namespace DART.Dartboard.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer t;

        private ILog _log = LogManager.GetLogger<MainWindow>();

        public MainWindow()
        {
            InitializeComponent();
            ConsoleOutput.Text = "";
            TextBoxLogger.GlobalLogTextBox = ConsoleOutput;

            HIDManager.SharedManager.AcquireAll();


            _log.Debug("Test");
            _log.Warn("Test");
            _log.Error("Test");
            _log.Fatal("Test");
            

            t = new Timer(50);
            t.Elapsed += TOnElapsed;
            t.Start();

            PrimaryStream.Source = new Uri("http://localhost:8080/");
            SecondaryStream.Source = new Uri("http://localhost:8080/");
        }

        private void TOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            HidDisplay.JoystickState = HIDManager.SharedManager.GetJoystickState();
            HidDisplay.GamepadState = HIDManager.SharedManager.GetGamepadState();
        }

    }
}
