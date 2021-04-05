using System;
using Code.Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    [Serializable]
    public class Thruster
    {
        [SerializeField] public Key KeyCode;
        [SerializeField] public float Power;
        [SerializeField] public Direction Direction;
        [SerializeField] public ThrusterBehavior ThrusterBehavior;

        private float _force;
        
        public void Execute(Rigidbody rigidbody, float force = 1f)
        {
            _force = force;
            
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
        
        private void Move(Rigidbody rigidbody) => rigidbody.AddRelativeForce(GetForceDirection() * (_force * Power));
        private void Roll(Rigidbody rigidbody) => rigidbody.AddRelativeTorque(GetTorqueDirection() * (_force * Power));
        
        private Vector3 GetForceDirection() =>
            Direction switch
            {
                Direction.Forward => Vector3.forward,
                Direction.Backward => Vector3.back,
                Direction.Left => Vector3.left,
                Direction.Right => Vector3.right,
                Direction.Up => Vector3.up,
                Direction.Down => Vector3.down,
                _ => throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null)
            };
        
        private Vector3 GetTorqueDirection() =>
            Direction switch
            {
                Direction.Forward => Vector3.right,
                Direction.Backward => Vector3.left,
                Direction.Left => Vector3.down,
                Direction.Right => Vector3.up,
                Direction.RollLeft => Vector3.forward,
                Direction.RollRight => Vector3.back,
                _ => throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null)
            };
    }
}