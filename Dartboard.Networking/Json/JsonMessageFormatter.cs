using System.Collections.Generic;
using System.Text;
using Dartboard.Networking.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dartboard.Networking.Json
{
    public class JsonMessageFormatter<T> : IMessageFormatter<T>
    {
        public bool IncludeTypes
        {
            get => _settings.TypeNameHandling == TypeNameHandling.All;
            set => _settings.TypeNameHandling = value ? TypeNameHandling.All : TypeNameHandling.None;
        }

        private JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            Converters = new List<JsonConverter>()
            {
                new Vector3Converter(),
                new ColorConverter(),
            },
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
        };

        public byte[] Format(T msg)
        {
            var jsonBody = JsonConvert.SerializeObject(msg, Formatting.None, _settings);

            return Encoding.UTF8.GetBytes(jsonBody + "\n");
        }

        public T Format(string msg)
        {
            return JsonConvert.DeserializeObject<T>(msg, _settings);
        }
    }
}
