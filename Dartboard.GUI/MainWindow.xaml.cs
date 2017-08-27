using System;
using System.Timers;
using System.Windows;
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


        public MainWindow()
        {
            InitializeComponent();
            t = new Timer(20);
            t.Elapsed += TOnElapsed;
            t.Start();

            PrimaryStream.Source = new Uri("http://localhost:8080/");
            SecondaryStream.Source = new Uri("http://localhost:8080/");
        }

        private void TOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Dispatcher.Invoke(() =>
            {
                //test.Value = HIDManager.SharedManager.GetJoystickState().Slider;
            });
        }

    }
}
