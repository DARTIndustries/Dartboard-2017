using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dartboard.TheBox
{
    public class Box
    {
        public static Box Locate()
        {
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                var sp = new SerialPort(port, 115200);
                try
                {
                    sp.Open();
                    sp.WriteLine("query");
                    if (sp.ReadLine() == "TheBox")
                        return new Box(sp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return null;
        }

        private SerialPort _port;

        public Box(string com)
        {
            _port = new SerialPort(com, 115200);
            _port.Open();
        }

        public Box(SerialPort port)
        {
            _port = port;
            if (!_port.IsOpen)
                throw new InvalidOperationException("Serial port must be opened");
        }

        public void SetColor(Microsoft.Xna.Framework.Color c)
        {
            byte[] buf = new byte[3];
            buf[0] = c.R;
            buf[1] = c.G;
            buf[2] = c.B;
            _port.Write(buf, 0, 3);
        }

        public void SetColor(Color c)
        {
            byte[] buf = new byte[3];
            buf[0] = c.R;
            buf[1] = c.G;
            buf[2] = c.B;
            _port.Write(buf, 0, 3);
        }
    }
}
