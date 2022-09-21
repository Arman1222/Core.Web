using Newtonsoft.Json;
using System;

namespace Core.Web.Helpers
{
    //http://stackoverflow.com/questions/27063475/custom-jsonconverter-that-can-convert-decimal-minvalue-to-empty-string-and-back
    public class EscapeCommaConverter : JsonConverter
    {
        private const string QUOTE = "\"";
        private const string ESCAPED_QUOTE = "\"\"";
        private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };

        //http://stackoverflow.com/questions/769621/dealing-with-commas-in-a-csv-file
        private string Escape(string s)
        {
            if (s.Contains(QUOTE))
                s = s.Replace(QUOTE, ESCAPED_QUOTE);

            if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
                s = QUOTE + s + QUOTE;

            return s;
        }
        private string Unescape(string s)
        {
            if (s.StartsWith(QUOTE) && s.EndsWith(QUOTE))
            {
                s = s.Substring(1, s.Length - 2);

                if (s.Contains(ESCAPED_QUOTE))
                    s = s.Replace(ESCAPED_QUOTE, QUOTE);
            }

            return s;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType,
                                        object existingValue, JsonSerializer serializer)
        {
            //if (reader.TokenType == JsonToken.String)
            //{
            //    if ((string)reader.Value == string.Empty)
            //    {
            //        return decimal.MinValue;
            //    }
            //}
            //else if (reader.TokenType == JsonToken.Float ||
            //         reader.TokenType == JsonToken.Integer)
            //{
            //    return Convert.ToDecimal(reader.Value);
            //}

            //throw new JsonSerializationException("Unexpected token type: " +
            //                                     reader.TokenType.ToString());

            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            string str = (string)value;            
            writer.WriteValue(Escape(str));
        }
    }
}
