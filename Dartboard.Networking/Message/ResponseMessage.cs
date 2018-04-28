using System;

namespace Dartboard.Networking.Message
{
    public class Heartbeat : Expireable
    {
        public Heartbeat(TimeSpan ttl) : base(ttl)
        {

        }
    }
}
