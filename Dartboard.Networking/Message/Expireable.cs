using System;
using Newtonsoft.Json;

namespace Dartboard.Networking.Message
{
    public abstract class Expireable
    {
        [JsonIgnore]
        public DateTime Expiration { get; }

        protected Expireable(TimeSpan ttl)
        {
            Expiration = DateTime.Now + ttl;
        }
    }
}