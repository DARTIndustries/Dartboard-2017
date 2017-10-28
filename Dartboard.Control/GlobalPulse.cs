using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;

namespace DART.Dartboard.Control
{
    public static class GlobalPulse
    {
        private static DateTime _lastPulse;
        private static Timer _timer;
        private static ILog _log = LogManager.GetLogger(typeof(GlobalPulse));

        public static event Action<TimeSpan> Pulse;

        private const int _defaultTime = 10;

        static GlobalPulse()
        {
            var time = _defaultTime;
            _timer = new Timer((state =>
            {
                Pulse?.Invoke(DateTime.Now - _lastPulse);
                _lastPulse = DateTime.Now;
            }), null, 0, time);
            _log.Debug($"Timer started with time interval of {time} ms");
        }

        public static void ChangeTimer(int newTime)
        {
            _timer.Change(0, newTime);
        }
    }
}
