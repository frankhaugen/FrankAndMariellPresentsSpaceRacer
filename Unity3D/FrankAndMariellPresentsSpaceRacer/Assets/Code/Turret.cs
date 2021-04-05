using System;
using System.Diagnostics;
using System.Linq;
using Code.Extensions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Code
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] public GameObject Target;
        
        [SerializeField]
        [Range(0f, 360f)]
        public float Precision;

        [SerializeField] 
        [Range(0f, 10f)]
        public float TurnSpeed;

        [SerializeField]
        [Range(0f, 1000f)]
        public float ProjectileSpeed;

        [SerializeField]
        [Range(1, 60)]
        public int ProjectileLifetime;

        private Stopwatch _stopwatch;
        private GameObject _aimpoint;

        private void Start()
        {
            _stopwatch = new Stopwatch();
            _aimpoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            _aimpoint.transform.localScale = _aimpoint.transform.localScale * 0.2f;
            _aimpoint.name = "Aimpoint";
            _aimpoint.GetComponent<MeshRenderer>().materials.First().color = Color.red;
        }

        void Update()
        {
            // var leadPosition = GetLeadPosition();
            var leadPosition = GetLeadPosition();
            _aimpoint.transform.position = leadPosition;

            
            // var targetDirection = Target.transform.position - transform.position;
            var targetDirection = leadPosition - transform.position;
            

            var targetDirectionRotation = Quaternion.LookRotation (targetDirection);
            var speed = Mathf.Min (TurnSpeed * Time.deltaTime, 1);
            var lerp = Quaternion.Lerp (transform.rotation, targetDirectionRotation, speed);
            var angle = Vector3.Angle(targetDirection, transform.forward);
            
            transform.rotation = lerp;
            if (_stopwatch.Elapsed.TotalSeconds > TimeSpan.FromSeconds(1).TotalSeconds)
            {
                _stopwatch.Stop();
                _stopwatch.Reset();
            }
            Debug.DrawLine(transform.position, _aimpoint.transform.position, Color.blue, 0.01f, false);

            if (angle < Precision && !_stopwatch.IsRunning)
            {
                _stopwatch.Start();
                // var ray = new Ray(transform.position, leadPosition);
                Debug.DrawRay(transform.position, targetDirection, Color.green, 1f, false);
                
                var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bullet.transform.position = transform.position;

                bullet.AddComponent<LifetimeLimiter>().TimeSpan = TimeSpan.FromSeconds(ProjectileLifetime);
                bullet.AddComponent<Projectile>();
                // bullet.transform.rotation = transform.rotation;
                bullet.transform.rotation = Quaternion.Euler(targetDirection);
                bullet.transform.localScale = Vector3.one * 0.4f; 
                
                var bulletRigidbody = bullet.AddComponent<Rigidbody>();
                bulletRigidbody.mass = 0f;
                bulletRigidbody.drag = 0f;
                bulletRigidbody.angularDrag = 0f;
                bulletRigidbody.useGravity = false;
                bulletRigidbody.velocity = transform.forward * ProjectileSpeed;
            }
        }

        private Vector3 GetLeadPosition()
        {
            var shotSpeed = ProjectileSpeed;
 
            var shooterPosition = transform.position;
            var targetPosition = Target.transform.position;
                
            var shooterVelocity = Vector3.zero;
            var targetVelocity = Target.GetComponent<Rigidbody>().velocity;

            var interceptPoint = shooterPosition.CalculateInterceptionPoint3D(shotSpeed, targetPosition, targetVelocity);
            
            // var interceptPoint = FirstOrderIntercept
            // (
            //     shooterPosition,
            //     shooterVelocity,
            //     shotSpeed,
            //     targetPosition,
            //     targetVelocity
            // );

            return interceptPoint;
        }
        
        //first-order intercept using absolute target position
        private static Vector3 FirstOrderIntercept
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
        private static float FirstOrderInterceptTime
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
