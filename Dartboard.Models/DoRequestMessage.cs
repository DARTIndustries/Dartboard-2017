using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DART.Dartboard.Models
{
    public class DoRequestMessage
    {
        public DoRequestMessage()
        {
        }

        public Do Do { get; set; }
        public Request Request { get; set; }
    }

    public class Do
    {
        public sbyte[] Motor { get; set; }
        
        public string Lights { get; set; }

        public Servo Servo { get; set; }

        public Camera Camera { get; set; }

    }
    
    public class Camera
    {
        public short[] Angles { get; set; }
        public sbyte[] Velocity { get; set; }
    }

    public class Servo
    {
        public short[] Angles { get; set; }
        public sbyte[] Velocity { get; set; }
    }

    public class Request
    {
        public int Id { get; set; }
        public string[] Fields { get; set; }
    }
}
