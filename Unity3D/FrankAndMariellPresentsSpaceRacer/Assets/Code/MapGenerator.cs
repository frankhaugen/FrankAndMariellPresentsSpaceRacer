using Code.IO.Json;
using Code.Libraries.Dynagon;
using Code.Models.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] public PlanetState PlanetState;

        private JsonContext<PlanetState> _context;
        
        // Start is called before the first frame update
        private void Start()
        {
            _context = new JsonContext<PlanetState>();

            // PlanetState = new PlanetState();
            PlanetState.Name = "Perle";
            PlanetState.Position = new Vector3(40f, 0f, 40f);
            PlanetState.Radius = 20f;
            
            _context.Add(PlanetState);
            _context.SaveChanges();

            GeneratePlanet();
        }

        private void GeneratePlanet()
        {
            var planet = _context.GetById(PlanetState.Id);
            var planetGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            planetGameObject.transform.position = planet.Position;
            planetGameObject.transform.localScale = new Vector3(planet.Radius, planet.Radius, planet.Radius);
        }
        
        private void GenerateSpheres()
        {
            for (var i = 0; i < 100; i++)
            {
                // var point = RandomPointInCircle(Random.Range(15f, 100f), Random.Range(0f, 45f));
                var point = RandomPointInCircle(new PolarCoordinates() { Radius = Random.Range(500f, 3000f), Azimuth = Random.Range(-15f, 15f), Inclination = Random.Range(-90f, 90f)});
                GenerateSphere("Hey, look at me", point);
                // GenerateMeteor("Hey, look at me", point);
            }
        }
    
        private void GenerateSphere(string sphereName, Vector3 position)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = sphereName;
            sphere.transform.position = position;
        }

        private Vector3 RandomPointInCircle(float radius, float angle){
            var rad = angle * Mathf.Deg2Rad;
            var position = new Vector3(Mathf.Sin( rad ), 0, Mathf.Cos( rad ));
            return position * radius;
        }

        private Vector3 RandomPointInCircle(PolarCoordinates polarCoordinates){
            var rad1 = polarCoordinates.Inclination * Mathf.Deg2Rad;
            var rad2 = polarCoordinates.Azimuth * Mathf.Deg2Rad;
            var position = new Vector3(Mathf.Sin( rad1 ), Mathf.Sin( rad2 ), Mathf.Cos( rad1 ));
            return position * polarCoordinates.Radius;
        }

        private void GenerateMeteors(int count = 500)
        {
            for (var i = 1; i < count; i++)
            {
                var position = new PolarCoordinates();
                var granulizer = 47.7f;
            
                position.Radius = Random.Range(100 * granulizer, 5000 * granulizer) / granulizer;
                position.Inclination = Random.Range(1 * granulizer, 360 * granulizer) / granulizer;
                position.Azimuth = Random.Range(1 * granulizer, 360 * granulizer) / granulizer;

                GenerateMeteor(i.ToString(), position);
            }
        }

        private void GenerateMeteor(string objectName, PolarCoordinates polarCoordinates)
        {
            var coordinates = SphericalToCartesian(polarCoordinates);
            GenerateMeteor(objectName, coordinates);
        }

        private void GenerateMeteor(string objectName, Vector3 coordinates)
        {
            var newObject = new GameObject();
            // newObject.transform.position = Random.insideUnitSphere * Random.Range(450, 500);
            newObject.transform.position = coordinates;
            newObject.name = objectName;
            var meteor = newObject.AddComponent<Meteor>();
            meteor.PolarCoordinates = CartesianToSpherical(coordinates);
            meteor.PolarCoordinates2 = CartesianToSpherical(coordinates);
            meteor.radius = Random.Range(1, 3);
            meteor.numVertices = Random.Range(9, 13);
        }
    
        public static PolarCoordinates CartesianToSpherical(Vector3 coordinates)
        {
            var result = new PolarCoordinates();
            result.Radius = Mathf.Sqrt((coordinates.x * coordinates.x)
                                       + (coordinates.y * coordinates.y)
                                       + (coordinates.z * coordinates.z));
            result.Inclination = Mathf.Acos(coordinates.z / result.Radius);
            result.Azimuth = Mathf.Atan(coordinates.y / coordinates.x);

            return result;
        }

        public static Vector3 SphericalToCartesian(PolarCoordinates polarCoordinates)
        {
            var result = new Vector3();
            result.x = polarCoordinates.Radius * Mathf.Sin(polarCoordinates.Inclination) * Mathf.Cos(polarCoordinates.Azimuth);
            result.y = polarCoordinates.Radius * Mathf.Sin(polarCoordinates.Inclination) * Mathf.Sin(polarCoordinates.Azimuth);
            result.z = polarCoordinates.Radius * Mathf.Cos(polarCoordinates.Inclination);
            return result;
        }
    }
}