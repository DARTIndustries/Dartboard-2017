using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.Control.GenericRobot;
using DART.Dartboard.Models.HID;
using Vector3D;

namespace DART.Dartboard.Control.Dart2017
{
    public class Robot2017 : Robot
    {
        public override Vector[] GetMotorVectors => new[]
        {
            new Vector(0.5, 0.5, 0),  // 0
            new Vector(-0.5, 0.5, 0), // 1
            new Vector(0, 0, 1),      // 2  
            new Vector(0, 0, 1),      // 3
            new Vector(-0.5, 0.5, 0), // 4
            new Vector(0.5, -0.5, 0), // 4
        };

        public override int NumberOfMotors => 6;
    }
}
