namespace ClearScriptDemo.Demo.SpawnDemo.Utils
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
            {
                return str;
            }

            char[] charArray = str.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                if (i == 1 && !char.IsUpper(charArray[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < charArray.Length);
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