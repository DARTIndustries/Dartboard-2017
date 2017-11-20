using System;
using System.IO;
using System.Windows;
using CefSharp;

namespace DART.Dartboard.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var settings = new CefSettings();

            settings.CefCommandLineArgs.Add("disable-gpu", "1");

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }
    }
}
