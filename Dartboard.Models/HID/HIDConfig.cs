using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DART.Dartboard.Models.HID
{
    public class HIDConfig
    {
        public bool FailOnNoDeviceFound { get; set; }
        public double? ControllerDeadZone { get; set; }
        public double? JoystickDeadZone { get; set; }
        public double? JoystickYawDeadZone { get; set; }
    }
}
