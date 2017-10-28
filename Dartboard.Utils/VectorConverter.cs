using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DART.Dartboard.Utils
{
    public class VectorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var token = JToken.Load(reader);
                var items = token.ToObject<double[]>();
                return Vector<double>.Build.DenseOfArray(items);
            }
            else
                return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector<double>);
        }
    }

    public class MatrixConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var token = JToken.Load(reader);
                var items = token.ToObject<double[]>();

                var itemsPerRow = (int) Math.Sqrt(items.Length);

                var rows = items
                    .Select((x, i) => new {Index = i, Value = x})
                    .GroupBy(x => x.Index / itemsPerRow)
                    .Select(x => x.Select(v => v.Value).ToArray()).ToArray();


                return Matrix<double>.Build.DenseOfRowArrays(rows);
            }
            else
                return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Matrix<double>);
        }
    }
}
