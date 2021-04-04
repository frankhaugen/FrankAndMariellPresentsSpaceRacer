using UnityEngine;

namespace Code.IO.Json
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T source, bool indented = true) where T : class
        {
            return JsonUtility.ToJson(source, indented);
        }

        public static T FromJson<T>(this string source) where T : class
        {
            return JsonUtility.FromJson<T>(source)!;
        }
    }
}
