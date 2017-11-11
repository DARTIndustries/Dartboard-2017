using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DART.Dartboard.Control
{
    public class ButtonWatcher
    {
        private bool _lastState;
        private bool _thisState;

        public void Update(bool current)
        {
            _lastState = _thisState;
            _thisState = current;
        }

        public bool RisingEdge()
        {
            return !_lastState && _thisState;
        }

        public bool FallingEdge()
        {
            return _lastState && !_thisState;
        }

        public bool Get()
        {
            return _thisState;
        }
    }

    public class WatchGroup
    {
        private readonly ConcurrentDictionary<int, ButtonWatcher> _watchers 
            = new ConcurrentDictionary<int, ButtonWatcher>();

        public ButtonWatcher this[int i]
        {
            get
            {
                if (_watchers.ContainsKey(i))
                    return _watchers[i];
                else
                {
                    _watchers.TryAdd(i, new ButtonWatcher());
                    return _watchers[i];
                }
            }
        }
    }
}
