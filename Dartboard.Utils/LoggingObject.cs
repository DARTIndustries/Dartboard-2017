using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Dartboard.Utils
{
    public abstract class LoggingObject
    {
        protected ILogger Log;

        protected LoggingObject()
        {
            Log = LogManager.GetCurrentClassLogger();
        }

    }
}
