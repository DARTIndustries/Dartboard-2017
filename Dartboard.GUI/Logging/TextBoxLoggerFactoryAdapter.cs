using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Simple;

namespace DART.Dartboard.GUI.Logging
{
    public class TextBoxLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    {
        public TextBoxLoggerFactoryAdapter(NameValueCollection properties) : base(properties)
        {
        }

        public TextBoxLoggerFactoryAdapter(LogLevel level, bool showDateTime, bool showLogName, bool showLevel, string dateTimeFormat) 
            : base(level, showDateTime, showLogName, showLevel, dateTimeFormat)
        {
        }

        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName,
            string dateTimeFormat)
        {
            return new TextBoxLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
        }
    }

    public  class TextBoxLogger : AbstractSimpleLogger
    {
        public static TextBox GlobalLogTextBox { get; set; }

        public TextBoxLogger(string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName, string dateTimeFormat) 
            : base(logName.Split('.').Last(), logLevel, showlevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            var sb = new StringBuilder();
            FormatOutput(sb, level, message, exception);

            if (GlobalLogTextBox != null)
            {
                GlobalLogTextBox.Dispatcher.Invoke(() =>
                {
                    GlobalLogTextBox.AppendText(sb + "\n");
                    if (Math.Abs(GlobalLogTextBox.VerticalOffset - (GlobalLogTextBox.ExtentHeight - GlobalLogTextBox.ViewportHeight)) < 0.01)
                    {
                        // At the end
                        GlobalLogTextBox.ScrollToEnd();
                    }
                });
            }
            else
                Console.WriteLine(sb.ToString());

        }
    }
}
