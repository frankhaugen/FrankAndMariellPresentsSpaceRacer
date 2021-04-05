using Code.Extensions;
using UnityEngine;

namespace Code
{
    public class Projectile : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            HandleCollision(other.gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            HandleCollision(other.gameObject);
        }

        private void HandleCollision(GameObject thing)
        {
            if (thing.IsPlayer())
            {
                Debug.Log(thing.name);
                Destroy(gameObject);
            }
        }
    }
}