using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.Models;
using Newtonsoft.Json;

namespace DART.Dartboard.Networking
{
    public class JsonMessageFormatter : IMessageFormatter
    {
        public byte[] Format(DoRequestMessage message)
        {
            return Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(message, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                }) + "\n");
        }
    }
}
