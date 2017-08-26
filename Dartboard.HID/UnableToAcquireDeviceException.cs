using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DART.Dartboard.HID
{
    public class UnableToAcquireDeviceException : Exception
    {
        public string Device { get; }

        public UnableToAcquireDeviceException(string device)
        {
            Device = device;
        }

        public UnableToAcquireDeviceException(string device, string message) : base(message)
        {
            Device = device;
        }
    }
}
