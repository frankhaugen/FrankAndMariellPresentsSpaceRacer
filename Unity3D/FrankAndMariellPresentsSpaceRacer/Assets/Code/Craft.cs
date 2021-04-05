using System;
using System.Collections.Generic;
using System.Linq;
using Code.Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Code
{
    public class Craft : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private IEnumerable<Mesh> _meshes;
        private Vector3 _localScale;
        private float _turnSpeedMultiplyer = 10f;
    
        [ReadOnly] public float Mass;
        [ReadOnly] public float Volume;
        [SerializeField] public float Density = 100;
        [SerializeField] public List<Thruster> Thrusters;

        private void Start()
        {
            _meshes = GetComponentsInChildren<MeshFilter>().Select(x => x.mesh);
        
            _localScale = GetComponent<Transform>().localScale;
            if (!gameObject.TryGetComponent(out _rigidbody)) _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;

            SetVolume();
            SetMass();
            
            _rigidbody.drag = Mathf.Epsilon;
        }
        
        private void FixedUpdate()
        {
            try
            {
                HandleInput();
            }
            catch (Exception e)
            {
                Debug.LogError(e);;
            }
            
            Thrusters.Where(x => InputSystem.GetDevice<Keyboard>().allKeys.FirstOrDefault(y => y.keyCode == x.KeyCode).IsPressed()).ToList().ForEach(x => x.Execute(_rigidbody));
        }

        private void HandleInput()
        {
            ApplyGamepadAxis();
        }

        private void ApplyGamepadAxis()
        {
            // Okay (translate left-right)
            Test(InputSystem.GetDevice<Gamepad>().leftStick.left.ReadValue(), Direction.Left, ThrusterBehavior.AddForce);
            Test(InputSystem.GetDevice<Gamepad>().leftStick.right.ReadValue(), Direction.Right, ThrusterBehavior.AddForce);
            
            // Okay (translate forward-backward)
            Test(InputSystem.GetDevice<Gamepad>().leftStick.up.ReadValue(), Direction.Forward, ThrusterBehavior.AddForce);
            Test(InputSystem.GetDevice<Gamepad>().leftStick.down.ReadValue(), Direction.Forward, ThrusterBehavior.AddForce);
            
            // Okay (yaw side-side)
            Test(InputSystem.GetDevice<Gamepad>().rightStick.left.ReadValue(), Direction.Left, ThrusterBehavior.AddTorque);
            Test(InputSystem.GetDevice<Gamepad>().rightStick.right.ReadValue(), Direction.Left, ThrusterBehavior.AddTorque);
            
            // Okay (pitch up-down)
            Test(InputSystem.GetDevice<Gamepad>().rightStick.up.ReadValue(), Direction.Up, ThrusterBehavior.AddTorque);
            Test(InputSystem.GetDevice<Gamepad>().rightStick.down.ReadValue(), Direction.Down, ThrusterBehavior.AddTorque);
        }

        private void Test(float input, Direction direction, ThrusterBehavior thrusterBehavior) => Thrusters.Where(x => x.Direction == direction && x.ThrusterBehavior == thrusterBehavior).ToList().ForEach(x => x.Execute(_rigidbody, input));
        
        private void Test(Vector2 input, Direction direction, ThrusterBehavior thrusterBehavior) => Thrusters.Where(x => x.Direction == direction && x.ThrusterBehavior == thrusterBehavior).ToList().ForEach(x => x.Execute(_rigidbody));

        private void SetVolume()
        {
            foreach (var mesh in _meshes)
            {
                Volume += VolumeOfMesh(mesh);
            }
        }

        private void SetMass()
        {
            _rigidbody.mass = CalculateMass(Density, Volume);
            Mass = _rigidbody.mass;
        }
    
        private float VolumeOfMesh(Mesh mesh)
        {
            float volume = 0;
            for (var i = 0; i < mesh.triangles.Length; i += 3)
            {
                var p1 = mesh.vertices[mesh.triangles[i + 0]];
                var p2 = mesh.vertices[mesh.triangles[i + 1]];
                var p3 = mesh.vertices[mesh.triangles[i + 2]];
                volume += SignedVolumeOfTriangle(p1, p2, p3);
            }

            volume *= _localScale.x * _localScale.y * _localScale.z;
            return Mathf.Abs(volume);
        }

        private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var v321 = p3.x * p2.y * p1.z;
            var v231 = p2.x * p3.y * p1.z;
            var v312 = p3.x * p1.y * p2.z;
            var v132 = p1.x * p3.y * p2.z;
            var v213 = p2.x * p1.y * p3.z;
            var v123 = p1.x * p2.y * p3.z;
            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }

        private static float CalculateMass(float density, float volume) => density * volume;
        private float CalculateDensity(float mass, float volume) => mass / volume;
        private float CalculateVolume(float mass, float density) => mass / density;
    }
}