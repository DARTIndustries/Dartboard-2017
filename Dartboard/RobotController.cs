using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dartboard.Integration;
using Dartboard.Networking;
using Dartboard.Networking.Message;
using Dartboard.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dartboard
{
    public class RobotController
    {
        private readonly AbstractRobot _robot;
        private readonly ConcurrentQueue<DoRequestMessage> _generatedMessages;
        private readonly ConcurrentQueue<object> _responseMessages;
        private readonly TimeSpan _messagePeriod;


        public RobotController(AbstractRobot robot, TimeSpan period)
        {
            _robot = robot;
            _messagePeriod = period;
            _generatedMessages = new ConcurrentQueue<DoRequestMessage>();
            _responseMessages = new ConcurrentQueue<object>();
        }

        public void Start()
        {
            var t = new Thread(MainControl);
            t.Start();
        }

        public DoRequestMessage PopMessage()
        {
            return _generatedMessages.TryDequeue(out var msg) ? msg : null;
        }

        public object PopResponse()
        {
            return _responseMessages.TryDequeue(out var msg) ? msg : null;
        }

        public void MainControl()
        {
            while (true)
            {
                EnsureTime(_messagePeriod, () =>
                {
                    // Pilot Controls
                    var pilot = GamePad.GetState(PlayerIndex.One);

                    var z = -pilot.Triggers.Left + pilot.Triggers.Right;
                    var moveVector = new Vector3(pilot.ThumbSticks.Left, z);

                    var headingVector = new Vector3(pilot.ThumbSticks.Right, 0);


                    // Copilot Controls
                    var copilot = GamePad.GetState(PlayerIndex.Two);
                    var camera = new ServoElement()
                    {
                        Velocity = new[]
                        {
                            Utility.ToSByte(copilot.ThumbSticks.Left.X),
                            Utility.ToSByte(copilot.ThumbSticks.Left.Y)
                        }
                    };

                    var claw = new ServoElement()
                    {
                        Velocity = new[]
                        {
                            Utility.ToSByte(-copilot.Triggers.Left + copilot.Triggers.Right)
                        }
                    };

                    var request = new DoRequestMessage
                    {
                        Do = new IndirectDoElement
                        {
                            Camera = camera,
                            Claw = claw,
                            Heading = headingVector,
                            Lights = Color.CornflowerBlue,
                            MovementVector = moveVector
                        }
                    };


                    _generatedMessages.Enqueue(request);

                });
            }
        }

        private void EnsureTime(TimeSpan minLength, Action action)
        {
            var start = DateTime.Now;
            action();
            var end = DateTime.Now;

            var length = end - start;

            if (length >= minLength)
                return;

            var wait = (int) (minLength - length).TotalMilliseconds;
            Thread.Sleep(wait);
        }
    }
}
