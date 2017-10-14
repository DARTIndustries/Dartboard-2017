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

        public DoRequestMessage(ProcessingFrame frame)
        {
            
        }

        public Do Do { get; set; }
        public Request Request { get; set; }
    }

    public class Do
    {
        public sbyte[] Motor { get; set; }
        
        public string Lights { get; set; }

    }

    public class Servo
    {
        public sbyte[] Angles { get; set; }
        public sbyte[] Velocity { get; set; }
    }

    public class Request
    {
        public int Id { get; set; }
        public string[] Fields { get; set; }
    }
}
