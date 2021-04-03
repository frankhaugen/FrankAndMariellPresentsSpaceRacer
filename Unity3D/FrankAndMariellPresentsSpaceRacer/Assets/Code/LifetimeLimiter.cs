using System;
using System.Diagnostics;
using UnityEngine;

namespace Code
{
    public class LifetimeLimiter : MonoBehaviour
    {
        public TimeSpan TimeSpan { get; set; }
        public Stopwatch Lifetime { get; set; }

        private void Start()
        {
            Lifetime = new Stopwatch();
            Lifetime.Start();
        }

        private void FixedUpdate()
        {
            if (Lifetime.Elapsed >= TimeSpan)
            {
                Destroy(gameObject);
            }
        }
    }
}