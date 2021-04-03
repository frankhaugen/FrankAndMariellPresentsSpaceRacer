using System;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class ThrusterConfiguration
    {
        [SerializeField] public float ForwardThrust;
        [SerializeField] public float BackwardThrust;
        [SerializeField] public float LeftThrust;
        [SerializeField] public float RightThrust;
        [SerializeField] public float UpThrust;
        [SerializeField] public float DownThrust;
    }
}