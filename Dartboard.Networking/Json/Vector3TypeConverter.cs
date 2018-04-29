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
            var conv = new [] { value.X.ToSByte(), value.Y.ToSByte(), value.Z.ToSByte() };
            serializer.Serialize(writer, conv);
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var array = serializer.Deserialize<sbyte[]>(reader);
            return new Vector3(array[0].ToFloat(), array[1].ToFloat(), array[2].ToFloat());
        }
    }
}
