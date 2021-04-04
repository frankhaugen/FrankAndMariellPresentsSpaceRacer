using System;
using UnityEngine;

namespace Code.Deprecated
{
    [Serializable]
    public class ShipConfiguration
    {
        [SerializeField] public float Volume;
        [SerializeField] public float Density;
        [SerializeField] public float Mass;
    }
}