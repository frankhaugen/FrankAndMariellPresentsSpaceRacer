using System;
using Code.Extensions;
using Code.IO.Json;
using Code.Models.Game;
using UnityEngine;

namespace Code
{
    public class WaypointScript : MonoBehaviour
    {
        private JsonContext<Waypoint> _context;

        private void Start()
        {
            _context = new JsonContext<Waypoint>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsPlayer())
            {
                var now = DateTimeOffset.UtcNow;
                _context.Add(new Waypoint()
                {
                    Stage = 1,
                    Timestamp = now.ToUnixTimeMilliseconds(),
                    DateTimeOffset = now.ToString(),
                    What = other.name
                });
            
                _context.SaveChanges();
                
                gameObject.SetActive(false);
            }
            else
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}