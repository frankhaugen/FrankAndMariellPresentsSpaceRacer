using System;
using Code.IO.Json;

namespace Code.Models.Game
{
    [Serializable]
    public class Waypoint : JsonEntity
    {
        public int Stage;
        public long Timestamp;
        public string DateTimeOffset;
        public string What;
    }
}