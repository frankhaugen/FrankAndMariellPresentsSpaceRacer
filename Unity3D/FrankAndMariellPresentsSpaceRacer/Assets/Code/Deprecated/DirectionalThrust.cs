using System;
using Code.Enums;
using UnityEngine;

namespace Code.Deprecated
{
    public class DirectionalThrust : MonoBehaviour
    {
        [SerializeField] public KeyCode Key;
        [SerializeField] public float Power = 1;
        [SerializeField] public Direction Direction;

        private Rigidbody _rigidbody;
        
        private void Start()
        {
            if (gameObject.TryGetComponent(out _rigidbody)) return;
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
        }

        private void FixedUpdate()
        {
            if (!Input.anyKey) return;
            if (Input.GetKey(Key)) Move();
        }

        private void Move() => _rigidbody.AddRelativeForce(GetDirection(Direction) * Power);

        private static Vector3 GetDirection(Direction direction) =>
            direction switch
            {
                Direction.Forward => Vector3.forward,
                Direction.Backward => Vector3.back,
                Direction.Left => Vector3.left,
                Direction.Right => Vector3.right,
                Direction.Up => Vector3.up,
                Direction.Down => Vector3.down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
    }
}
