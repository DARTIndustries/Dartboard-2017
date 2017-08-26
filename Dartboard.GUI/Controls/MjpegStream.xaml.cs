using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly MjpegDecoder _decoder;

        protected ILog Log;

        public MjpegStream()
        {
            InitializeComponent();
            _decoder = new MjpegDecoder();
            _decoder.FrameReady += DecoderOnFrameReady;
            _decoder.Error += DecoderOnError;
            Log = LogManager.GetLogger(GetType());
        }

        private void DecoderOnError(object sender, ErrorEventArgs errorEventArgs)
        {
            Log.Error(errorEventArgs.Message);
        }

        private void DecoderOnFrameReady(object sender, FrameReadyEventArgs frameReadyEventArgs)
        {
            canvas.Source = _decoder.BitmapImage;
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(MjpegStream));


        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set
            {
                _source = value;
                _decoder.StopStream();
                _decoder.ParseStream(value);
            }
        }
    }
}
