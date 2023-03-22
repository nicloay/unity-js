using ClearScriptDemo.JSonConverters;
using Newtonsoft.Json;

namespace ClearScriptDemo.Demo.SpawnDemo.Utils
{
    public static class StringExtensions
    {
        private static readonly MessageConverter MESSAGE_CONVERTER = new();

        public static string ToJsonString(this IMessage message)
        {
            return JsonConvert.SerializeObject(message, MESSAGE_CONVERTER);
        }

        public static IMessage ToMessage(this string str)
        {
            return JsonConvert.DeserializeObject<IMessage>(str, MESSAGE_CONVERTER);
        }

        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0])) return str;

            var charArray = str.ToCharArray();
            for (var i = 0; i < charArray.Length; i++)
            {
                if (i == 1 && !char.IsUpper(charArray[i])) break;

                var hasNext = i + 1 < charArray.Length;
                if (i > 0 && hasNext && !char.IsUpper(charArray[i + 1]))
                {
                    charArray[i] = char.ToLowerInvariant(charArray[i]);
                    break;
                }

                charArray[i] = char.ToLowerInvariant(charArray[i]);
            }

            return new string(charArray);
        }
    }
}