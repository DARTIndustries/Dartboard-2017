using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dartboard.Networking.Message;

namespace Dartboard.Networking
{
    public abstract class AbstractNetworkClient
    {
        public abstract void Send(DoRequestMessage msg);
        public Action<Heartbeat> Heartbeat;
    }
}
