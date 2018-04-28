using System;
using Dartboard.Utils;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dartboard.Networking.Json
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            var conv = new [] { value.X.ToByte(), value.Y.ToByte(), value.Z.ToByte() };
            serializer.Serialize(writer, conv);
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var array = serializer.Deserialize<sbyte[]>(reader);
            return new Vector3(array[0].ToFloat(), array[1].ToFloat(), array[2].ToFloat());
        }
    }

    public class ColorConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteValue("#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2"));
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;
    }
}
