using System;
using System.Collections.Generic;

namespace Code.IO.Json
{
    [Serializable]
    public class JsonWrapper<T>
    {
        public List<T> Entities;
    }
}