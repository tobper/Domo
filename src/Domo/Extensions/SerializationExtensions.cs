using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Domo.Extensions
{
    public static class SerializationExtensions
    {
        public static TValue ParseJson<TValue>(this string data)
        {
            var serializer = CreateJsonSerializer<TValue>();

            return Deserialize<TValue>(data, serializer.ReadObject);
        }

        public static TValue ParseXml<TValue>(this string text)
        {
            var serializer = CreateXmlSerializer<TValue>();

            return Deserialize<TValue>(text, serializer.ReadObject);
        }

        public static string ToJson<TValue>(this TValue value)
        {
            var serializer = CreateJsonSerializer<TValue>();

            return Serialize(value, serializer.WriteObject);
        }

        public static string ToXml<TValue>(this TValue value)
        {
            var serializer = CreateXmlSerializer<TValue>();

            return Serialize(value, serializer.WriteObject);
        }

        private static DataContractJsonSerializer CreateJsonSerializer<TValue>()
        {
            var valueType = typeof(TValue);
            var serializer = new DataContractJsonSerializer(valueType);
            return serializer;
        }

        private static DataContractSerializer CreateXmlSerializer<TValue>()
        {
            var valueType = typeof(TValue);
            var serializer = new DataContractSerializer(valueType);

            return serializer;
        }

        private static string Serialize(object value, Action<Stream, object> serialize)
        {
            using (var memoryStream = new MemoryStream())
            {
                serialize(memoryStream, value);

                var buffer = memoryStream.ToArray();
                var result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                return result;
            }
        }

        private static TValue Deserialize<TValue>(string data, Func<Stream, object> deserialize)
        {
            var buffer = Encoding.UTF8.GetBytes(data);

            using (var memoryStream = new MemoryStream(buffer))
            {
                return (TValue)deserialize(memoryStream);
            }
        }
    }
}