using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common.Logging;
using MjpegProcessor;

namespace DART.Dartboard.GUI.Controls
{
    /// <summary>
    /// Interaction logic for MjpegStream.xaml
    /// </summary>
    public partial class MjpegStream : UserControl
    {
        protected ILog Log;

        public MjpegStream()
        {
            InitializeComponent();
            Log = LogManager.GetLogger(GetType());

            vlc.MediaPlayer.VlcLibDirectory = new DirectoryInfo(@"c:\Program Files (x86)\VideoLAN\VLC\");
            vlc.MediaPlayer.VlcMediaplayerOptions = new[] {"--network-caching=0"};
            vlc.MediaPlayer.EndInit();
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(MjpegStream));

        public void Start()
        {
            vlc.MediaPlayer.Play(Source);
        }

        public Uri Source { get; set; }
    }
}
