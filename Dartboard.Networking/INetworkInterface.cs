using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.Models;

namespace DART.Dartboard.Networking
{
    public interface INetworkInterface
    {
        void Send(Do message);
    }

    public interface IMessageFormatter
    {
        byte[] Format(DoRequestMessage message);
    }
}
