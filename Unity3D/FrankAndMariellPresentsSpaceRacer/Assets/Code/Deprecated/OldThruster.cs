using System.Collections.Generic;
using Code.Enums;
using UnityEngine;

namespace Code.Deprecated
{
    public class OldThruster : MonoBehaviour
    {
        [SerializeField] public KeyCode Key;
        [SerializeField] public float Power;
        [SerializeField] public readonly float Multiplier = 1000;
        [SerializeField] public Dictionary<Direction, Vector3> Directions;
    
        // Update is called once per frame
        private void FixedUpdate()
        {
            if (Input.anyKey)
            {
                if (Input.GetKey(Key)) MoveUp(Multiplier);
                // if (Input.GetKey(KeyCode.D)) MoveRight(ThrusterConfiguration.RightThrust);
                // if (Input.GetKey(KeyCode.W)) MoveForward(ThrusterConfiguration.ForwardThrust);
                // if (Input.GetKey(KeyCode.S)) MoveBackward(ThrusterConfiguration.BackwardThrust);
                // if (Input.GetKey(KeyCode.Space)) MoveUp(ThrusterConfiguration.UpThrust);
                // if (Input.GetKey(KeyCode.C))  MoveDown(ThrusterConfiguration.DownThrust);
            
                // if (Input.GetKey(KeyCode.LeftArrow)) YawLeft(ThrusterConfiguration.LeftThrust + ThrusterConfiguration.RightThrust / 4);
                // if (Input.GetKey(KeyCode.RightArrow)) YawRight(ThrusterConfiguration.LeftThrust + ThrusterConfiguration.RightThrust / 4);
                // if (Input.GetKey(KeyCode.UpArrow)) PitchDown(ThrusterConfiguration.UpThrust + ThrusterConfiguration.DownThrust / 4);
                // if (Input.GetKey(KeyCode.DownArrow)) PitchUp(ThrusterConfiguration.UpThrust + ThrusterConfiguration.DownThrust / 4);
                // if (Input.GetKey(KeyCode.Q)) RollLeft(ThrusterConfiguration.LeftThrust + ThrusterConfiguration.RightThrust / 4);
                // if (Input.GetKey(KeyCode.E)) RollRight(ThrusterConfiguration.LeftThrust + ThrusterConfiguration.RightThrust / 4);
            }
        }
    
        private void MoveLeft(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.left * multiplier);
        private void MoveRight(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * multiplier);
        private void MoveForward(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * multiplier);
        private void MoveBackward(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * multiplier);
        private void MoveUp(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * multiplier);
        private void MoveDown(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * multiplier);
    
        private void PitchDown(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.right * multiplier);
        private void PitchUp(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.left * multiplier);
        private void YawLeft(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.down * multiplier);
        private void YawRight(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * multiplier);
        private void RollLeft(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.forward * multiplier);
        private void RollRight(float multiplier = 1000) => gameObject.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.back * multiplier);
    }
}