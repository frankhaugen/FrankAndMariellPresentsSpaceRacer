using UnityEngine;

namespace Code.WorldGeneration
{
    public class WaypointGenerator : MonoBehaviour
    {
        [SerializeField] public float MaxXVariation;
        [SerializeField] public  float MaxYVariation;
        [SerializeField] public  float MaxZVariation;
        [SerializeField] public  float MinZVariation;
        [SerializeField] public  int NumberOfWaypoints;

        private Vector3 _lastPosition = Vector3.zero;
        
        private void Start()
        {
            for (var i = 0; i < NumberOfWaypoints; i++)
            {
                CreateWaypoint();
            }
        }

        private void CreateWaypoint()
        {
            var newPosition = _lastPosition;

            newPosition.x = Random.Range(-MaxXVariation, MaxXVariation);
            newPosition.y = Random.Range(-MaxYVariation, MaxYVariation);
            newPosition.z = Random.Range(MinZVariation, MaxZVariation);

            GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = newPosition;
            
            _lastPosition = newPosition;
        }
    }
}