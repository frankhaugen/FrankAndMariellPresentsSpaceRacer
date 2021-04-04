using System;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class PolarCoordinates
    {
        /// <summary>
        /// Azimuth
        /// </summary>
        [SerializeField] public float Azimuth;
    
        /// <summary>
        /// Inclination
        /// </summary>
        [SerializeField] public float Inclination;

        /// <summary>
        /// Distance
        /// </summary>
        [SerializeField] public float Radius;
    }
}