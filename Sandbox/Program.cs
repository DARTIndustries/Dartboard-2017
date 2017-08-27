using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DART.Dartboard.HID;
using System.Net.Sockets;
using Dartboard.Utils;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = HIDManager.SharedManager;
            manager.AcquireAll();
            while (true)
            {
                var state1 = manager.GetGamepadState();
                var state2 = manager.GetJoystickState();
                PropPrint(state1);
                PropPrint(state2);
                Thread.Sleep(50);
                Console.Clear();
            }
        }


        static void PropPrint(object o)
        {
            if (o == null)
                return;

            var t = o.GetType();
            Console.WriteLine(t.Name);
            var fields = t.GetFields();
            foreach (var fieldInfo in fields)
            {
                Console.WriteLine(fieldInfo.Name + ": " + fieldInfo.GetValue(o));
            }

            var props = t.GetProperties();
            foreach (var propertyInfo in props)
            {
                if (propertyInfo.PropertyType == typeof(int[]))
                {
                    Console.WriteLine(propertyInfo.Name + ": " + string.Join(", ", (int[])propertyInfo.GetValue(o)));
                }
                else if (propertyInfo.PropertyType == typeof(bool[]))
                {
                    var ba = (bool[]) propertyInfo.GetValue(o);
                    if (ba.Length > 5)
                    {
                        Console.WriteLine(propertyInfo.Name + ": ");
                        for (int i = 0; i < Math.Min(ba.Length, 15); i++)
                        {
                            Console.WriteLine("  {0}: {1}", i, ba[i]);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(propertyInfo.Name + ": " + propertyInfo.GetValue(o));
                }
            }
            Console.WriteLine();
        }
    }
}
