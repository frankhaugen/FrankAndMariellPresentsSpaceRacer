using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Physics;
using System;

namespace FrankAndMariellPresentsSpaceRacer
{
    public class Thruster
    {
        public Keys Key { get; set; }
        public float Power { get; set; }
        public Direction Direction { get; set; }
        public ThrusterBehavior ThrusterBehavior { get; set; }

        public void Execute(RigidbodyComponent rigidbody)
        {
            switch (ThrusterBehavior)
            {
                case ThrusterBehavior.AddForce:
                    Move(rigidbody);
                    break;
                case ThrusterBehavior.AddTorque:
                    Roll(rigidbody);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Move(RigidbodyComponent rigidbody) => rigidbody.ApplyForce(GetDirection() * Power);
        private void Roll(RigidbodyComponent rigidbody) => rigidbody.ApplyTorque(GetDirection() * Power);

        private Vector3 GetDirection()
        {
            switch (Direction)
            {
                case Direction.Forward:
                    return Vector3.UnitZ;
                case Direction.Backward:
                    return -Vector3.UnitZ;
                case Direction.Left:
                    return -Vector3.UnitX;
                case Direction.Right:
                    return Vector3.UnitX;
                case Direction.Up:
                    return Vector3.UnitY;
                case Direction.Down:
                    return -Vector3.UnitY;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null);
            }
        }
    }
}
