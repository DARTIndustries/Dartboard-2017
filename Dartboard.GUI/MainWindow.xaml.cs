using System;
using System.Windows;
using MjpegProcessor;

namespace DART.Dartboard.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            left.Source = new Uri("http://129.25.217.182:8080/?action=stream_0");
            right.Source = new Uri("http://129.25.217.182:8080/?action=stream_1");
        }
    }
}
