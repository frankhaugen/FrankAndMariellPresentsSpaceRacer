using System;

namespace Code.IO.Json
{
    [Serializable]
    public class JsonEntity
    {
        public Guid Id;

        public JsonEntity()
        {
            Id = Guid.NewGuid();;
        }
    }
}
