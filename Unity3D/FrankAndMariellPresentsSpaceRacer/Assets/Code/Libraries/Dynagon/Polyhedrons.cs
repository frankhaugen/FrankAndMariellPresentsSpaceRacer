using System.Collections.Generic;
using UnityEngine;

namespace Code.Libraries.Dynagon {

	public class Polyhedrons : MonoBehaviour {

		public float interval = 0.5f;
		public float floorSize = 5f;

		private float timer = 0f;

		private class Polyhedron {

			private static Polyhedron pyramid = new Pyramid();
			private static Polyhedron tetrahedron = new Tetrahedron();
			private static Polyhedron octahedron = new Octahedron();
			private static Polyhedron[] polyhedrons = {pyramid, tetrahedron, octahedron};
			
			public static Polyhedron GetRandom() {
				return polyhedrons[Random.Range(0, polyhedrons.Length)];
			}

			public static Polyhedron GetPyramid() {
				return pyramid;
			}
			
			private Polyhedron() {}
			
			public virtual Polygon Create(float size = 1f) {
				return null;
			}

			private class Pyramid : Polyhedron {
				public override Polygon Create(float size = 1f) {
					var eulerX = (Mathf.PI / 2 + Mathf.Asin(1f/3f)) * Mathf.Rad2Deg;
					var eulerY = 90;
					var vertices = new List<Vector3>() {
						Vector3.up * size,
						Quaternion.Euler(eulerX, 0, 0) * Vector3.up * size,
						Quaternion.Euler(eulerX, eulerY * 1, 0) * Vector3.up * size,
						Quaternion.Euler(eulerX, eulerY * 2, 0) * Vector3.up * size,
						Quaternion.Euler(eulerX, eulerY * 3, 0) * Vector3.up * size
					};
					return Factory.Create("Pyramid", vertices);
				}
			}

			private class Tetrahedron : Polyhedron {
				public override Polygon Create(float size = 1f) {
					var eulerX = (Mathf.PI / 2 + Mathf.Asin(1f/3f)) * Mathf.Rad2Deg;
					var eulerY = 120;
					var vertices = new List<Vector3>() {
						Vector3.up * size,
						Quaternion.Euler(eulerX, 0, 0) * Vector3.up * size,
						Quaternion.Euler(eulerX, eulerY * 1, 0) * Vector3.up * size,
						Quaternion.Euler(eulerX, eulerY * 2, 0) * Vector3.up * size
					};
					return Factory.Create("Tetrahedron", vertices);
				}
			}

			private class Octahedron : Polyhedron {
				public override Polygon Create(float size = 1f) {
					var eulerX = 90f;
					var eulerY = 90f;
					var vertices = new List<Vector3>() {
						Vector3.up * size,
						Quaternion.Euler(eulerX, 0, 0) * Vector3.up * size,
						Quaternion.Euler(eulerX, eulerY * 1, 0) * Vector3.up * size,
						Quaternion.Euler(eulerX, eulerY * 2, 0) * Vector3.up * size,
						Quaternion.Euler(eulerX, eulerY * 3, 0) * Vector3.up * size,
						Vector3.up * -size
					};
					return Factory.Create("Octahedron", vertices);
				}
			}
			
		}

		private void Create() {
			var polygon = Polyhedron.GetRandom().Create();
			polygon.gameObject.transform.position = new Vector3(0, 8, 0) + Random.insideUnitSphere;
			polygon.Rigidize();
		}

		private void Start() {
			Polyhedron.GetPyramid().Create(floorSize);
		}

		private void Update() {
			timer += Time.deltaTime;
			if (timer > interval) {
				timer = 0f;
				Create();
			}
		}

	}

}
