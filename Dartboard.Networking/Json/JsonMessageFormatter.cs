using System.Collections.Generic;
using System.Text;
using Dartboard.Networking.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dartboard.Networking.Json
{
    public class JsonMessageFormatter : IMessageFormatter
    {
        private readonly List<JsonConverter> converters = new List<JsonConverter>
        {
            new Vector3Converter(),
            new ColorConverter(), 
        };

        public byte[] Format(DoRequestMessage msg)
        {
            var jsonBody = JsonConvert.SerializeObject(msg, Formatting.None, new JsonSerializerSettings()
            {
                Converters = converters,
                NullValueHandling = NullValueHandling.Ignore
            });

            return Encoding.UTF8.GetBytes(jsonBody + "\n");
        }

        public Heartbeat Format(string msg)
        {
            return JsonConvert.DeserializeObject<Heartbeat>(msg);
        }
    }
}
