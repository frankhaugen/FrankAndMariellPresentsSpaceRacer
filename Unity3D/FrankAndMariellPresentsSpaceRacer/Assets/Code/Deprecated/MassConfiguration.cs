using System;
using UnityEngine;

namespace Code.Deprecated
{
    [Serializable]
    public class MassConfiguration
    {
        [SerializeField] public float MassMultiplier;
        [SerializeField] public float Volume;
        [SerializeField] public float Density;
    }
}