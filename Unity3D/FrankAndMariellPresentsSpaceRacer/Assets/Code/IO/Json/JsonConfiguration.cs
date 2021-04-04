using System.IO;

namespace Code.IO.Json
{
    public class JsonConfiguration
    {
        public string Folder { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "Data");
    }
}
