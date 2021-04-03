using System;
using System.Diagnostics;
using UnityEngine;

namespace Code
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] public GameObject Target;
        [SerializeField] public float Strength;
        
        [SerializeField]
        [Range(0f, 360f)]
        public float Precision;

        private Stopwatch _stopwatch;

        private void Start()
        {
            _stopwatch = new Stopwatch();
        }

        void Update()
        {
            var targetDirection = Target.transform.position - transform.position;

            var targetDirectionRotation = Quaternion.LookRotation (targetDirection);
            var speed = Mathf.Min (Strength * Time.deltaTime, 1);
            var lerp = Quaternion.Lerp (transform.rotation, targetDirectionRotation, speed);
            var angle = Vector3.Angle(targetDirection, transform.forward);

            
            
            transform.rotation = lerp;
            if (_stopwatch.Elapsed.TotalSeconds > TimeSpan.FromSeconds(1).TotalSeconds)
            {
                _stopwatch.Stop();
                _stopwatch.Reset();
            }
            
            if (angle < Precision && !_stopwatch.IsRunning)
            {
                _stopwatch.Start();
                
                var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bullet.transform.position = transform.forward;
                bullet.AddComponent<LifetimeLimiter>().TimeSpan = TimeSpan.FromSeconds(3);
                bullet.transform.rotation = transform.rotation;
                bullet.transform.localScale = Vector3.one * 0.4f; 
                
                var bulletRigidbody = bullet.AddComponent<Rigidbody>();
                bulletRigidbody.mass = 10f;
                bulletRigidbody.useGravity = false;
                bulletRigidbody.velocity = Vector3.ClampMagnitude(transform.forward, 1f) * 10f;
            }
        }

        private Vector3 GetLeadPosition()
        {
            var shotSpeed = 10f;
 
            var shooterPosition = transform.position;
            var targetPosition = Target.transform.position;
                
            var shooterVelocity = Vector3.zero;
            var targetVelocity = Target.GetComponent<Rigidbody>() ? Target.GetComponent<Rigidbody>().velocity : Vector3.zero;
 
            var interceptPoint = FirstOrderIntercept
            (
                shooterPosition,
                shooterVelocity,
                shotSpeed,
                targetPosition,
                targetVelocity
            );

            return interceptPoint;
        }
        
        //first-order intercept using absolute target position
        public static Vector3 FirstOrderIntercept
        (
            Vector3 shooterPosition,
            Vector3 shooterVelocity,
            float shotSpeed,
            Vector3 targetPosition,
            Vector3 targetVelocity
        )
        {
            var targetRelativePosition = targetPosition - shooterPosition;
            var targetRelativeVelocity = targetVelocity - shooterVelocity;
            var t = FirstOrderInterceptTime
            (
                shotSpeed,
                targetRelativePosition,
                targetRelativeVelocity
            );
            return targetPosition + t*(targetRelativeVelocity);
        }
        
        //first-order intercept using relative target position
        public static float FirstOrderInterceptTime
        (
            float shotSpeed,
            Vector3 targetRelativePosition,
            Vector3 targetRelativeVelocity
        )
        {
            var velocitySquared = targetRelativeVelocity.sqrMagnitude;
            if(velocitySquared < 0.001f)
                return 0f;
 
            var a = velocitySquared - shotSpeed*shotSpeed;
 
            //handle similar velocities
            if (Mathf.Abs(a) < 0.001f)
            {
                var t = -targetRelativePosition.sqrMagnitude/
                        (
                            2f*Vector3.Dot
                            (
                                targetRelativeVelocity,
                                targetRelativePosition
                            )
                        );
                return Mathf.Max(t, 0f); //don't shoot back in time
            }
 
            var b = 2f*Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
            var c = targetRelativePosition.sqrMagnitude;
            var determinant = b*b - 4f*a*c;
 
            if (determinant > 0f) { //determinant > 0; two intercept paths (most common)
                float	t1 = (-b + Mathf.Sqrt(determinant))/(2f*a),
                    t2 = (-b - Mathf.Sqrt(determinant))/(2f*a);
                if (t1 > 0f) {
                    if (t2 > 0f)
                        return Mathf.Min(t1, t2); //both are positive
                    else
                        return t1; //only t1 is positive
                } else
                    return Mathf.Max(t2, 0f); //don't shoot back in time
            } else if (determinant < 0f) //determinant < 0; no intercept path
                return 0f;
            else //determinant = 0; one intercept path, pretty much never happens
                return Mathf.Max(-b/(2f*a), 0f); //don't shoot back in time
        }
    }
}
