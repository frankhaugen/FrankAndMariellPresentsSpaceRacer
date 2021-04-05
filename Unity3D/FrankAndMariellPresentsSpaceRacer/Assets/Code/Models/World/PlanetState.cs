using System;
using Code.IO.Json;
using UnityEngine;

namespace Code.Models.World
{
    [Serializable]
    public class PlanetState : JsonEntity
    {
        // public Guid Id;
        public string Name;
        public Vector3 Position;
        public float Radius;
    }
}