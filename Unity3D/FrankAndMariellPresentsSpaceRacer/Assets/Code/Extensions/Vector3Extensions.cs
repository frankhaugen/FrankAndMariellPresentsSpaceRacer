using UnityEngine;

namespace Code.Extensions
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// <para>Since Laser speed is constant no need to calculate relative speed of laser to get interception pos!</para>
        /// <para>Calculates interception point between two moving objects where chaser speed is known but chaser vector is not known(Angle to fire at * LaserSpeed"*Sort of*")</para>
        /// <para>Can use System.Math and doubles to make this formula NASA like precision.</para>
        /// </summary>
        /// <param name="turretPosition">Turret position</param>
        /// <param name="projectileSpeed">Speed of laser</param>
        /// <param name="targetPosition">Target initial position</param>
        /// <param name="targetVelocityVector">Target velocity vector</param>
        /// <returns>Interception Point as World Position</returns>
        public static Vector3 CalculateInterceptionPoint3D(this Vector3 turretPosition, float projectileSpeed, Vector3 targetPosition, Vector3 targetVelocityVector)
        {
            //! Distance between turret and target
            var distance = turretPosition - targetPosition;
 
            //! Scale of distance vector
            // var d = distance.magnitude;
 
            //! Speed of target scale of VR
            var targetVelocityVectorMagnitude = targetVelocityVector.magnitude;
 
            //% Quadratic EQUATION members = (ax)^2 + bx + c = 0
 
            var a = Mathf.Pow(projectileSpeed, 2) - Mathf.Pow(targetVelocityVectorMagnitude, 2);
 
            var b = 2 * Vector3.Dot(distance, targetVelocityVector);
 
            var c = -Vector3.Dot(distance, distance);
 
            if ((Mathf.Pow(b, 2) - (4 * (a * c))) < 0) //% The QUADRATIC FORMULA will not return a real number because sqrt(-value) is not a real number thus no interception
            {
                return Vector2.zero;//TODO: HERE, PREVENT TURRET FROM FIRING LASERS INSTEAD OF MAKING LASERS FIRE AT ZERO!
            }
            //% Quadratic FORMULA = x = (  -b+sqrt( ((b)^2) * 4*a*c )  ) / 2a
            var t = (-(b) + Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * (a * c)))) / (2 * a);//% x = time to reach interception point which is = t
 
            //% Calculate point of interception as vector from calculating distance between target and interception by t * VelocityVector
            return ((t * targetVelocityVector) + targetPosition);
        }
    }
}