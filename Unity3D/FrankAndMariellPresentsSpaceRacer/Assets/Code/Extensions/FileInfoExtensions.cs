using System.IO;
using Code.IO;
using Code.IO.Json;

namespace Code.Extensions
{
    public static class FileInfoExtensions
    {
        public static void WriteString(this FileInfo fileInfo, string @string) => FileHandler.WriteString(@string, fileInfo.FullName);
        public static void WriteObject<T>(this FileInfo fileInfo, T value) where T : class => WriteString(fileInfo, value.ToJson());
        
        public static string ReadString(this FileInfo fileInfo) => FileHandler.ReadString(fileInfo.FullName);
        public static T ReadObject<T>(this FileInfo fileInfo) where T : class => ReadString(fileInfo).FromJson<T>();
    }
}