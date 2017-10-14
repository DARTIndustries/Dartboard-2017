using System;
using System.Timers;
using System.Windows;
using Common.Logging;
using DART.Dartboard.Control;
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



            PrimaryStream.Source = new Uri("http://129.25.217.182:8080/?action=stream_0");
            PrimaryStream.Start();
            SecondaryStream.Source = new Uri("http://129.25.217.182:8080/?action=stream_1");
            SecondaryStream.Start();
        }

        private void GlobalPulseOnPulse(TimeSpan timeSpan)
        {
            HidDisplay.JoystickState = HIDManager.SharedManager.GetJoystickState();
            HidDisplay.GamepadState = HIDManager.SharedManager.GetGamepadState();
        }
    }
}
