using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.HID;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var wrapper = new JoystickWrapper(DeviceType.FirstPerson);
            if (wrapper.Acquired)
            {
                while (true)
                {
                    wrapper.Poll();
                    Console.WriteLine(wrapper._currentState.X);
                }
            }

            Console.ReadLine();
        }
    }
}
