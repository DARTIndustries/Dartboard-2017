using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Dartboard.Networking.Json
{
    public class ColorConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteValue("#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2"));
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;

            if (value == null)
                return Color.Black;
            // #rrggbb

            var r = value.Substring(1, 2);
            var g = value.Substring(3, 2);
            var b = value.Substring(5, 2);

            var rb = byte.Parse(r, NumberStyles.HexNumber);
            var gb = byte.Parse(g, NumberStyles.HexNumber);
            var bb = byte.Parse(b, NumberStyles.HexNumber);

            return new Color(rb, gb, bb);
        }
    }
}