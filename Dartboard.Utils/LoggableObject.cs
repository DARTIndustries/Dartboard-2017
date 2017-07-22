using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Dartboard.Utils
{
    public abstract class LoggableObject
    {
        protected ILog Log;

        protected LoggableObject()
        {
            Log = LogManager.GetLogger(GetType());
        }
    }
}
