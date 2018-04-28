using System;

namespace Dartboard.Networking.Message
{

    public class DoRequestMessage : Expireable
    {
        public DoRequestMessage(TimeSpan ttl) : base(ttl)
        {
            //Do = new DoElement();

            //Request = new RequestElement();

            //Config = new ConfigElement();
        }

        public DoElement Do { get; set; }

        public RequestElement Request { get; set; }

        public ConfigElement Config { get; set; }
    }
}