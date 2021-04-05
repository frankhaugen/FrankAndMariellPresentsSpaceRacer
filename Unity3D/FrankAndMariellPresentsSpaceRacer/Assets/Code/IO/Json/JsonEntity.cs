using System;

namespace Code.IO.Json
{
    [Serializable]
    public abstract class JsonEntity
    {
        public ulong Id;
        public string SessionId;
    }
}
