using System.Collections.Generic;
using UnityEngine;

namespace Code.Libraries.Dynagon {

	public class Meteor : MonoBehaviour {
		
		public int numVertices = 9;
		public float radius = 1.5f;

		[SerializeField]
		public PolarCoordinates PolarCoordinates;

		[SerializeField]
		public PolarCoordinates PolarCoordinates2;

		private List<Vector3> vertices = new List<Vector3>();
		private Polygon polygon;

		private void Start() {
			for (var i = 0; numVertices > i; i++) {
				vertices.Add(Random.onUnitSphere * radius);
			}
			polygon = Factory.Create(gameObject, vertices);
		}

		private void XOnCollisionEnter(Collision col) {
			var contact = col.gameObject.transform.position - polygon.gameObject.transform.position;
			Split(contact);
		}

		private void Split(Vector3 onePoint) {
			var p = new int[3];
			p[0] = GetNearest(onePoint);
			p[1] = GetFarthest(vertices[p[0]]);
			p[2] = GetFarthest((vertices[p[0]] + polygon.vertices[p[1]]) / 2);
			
			var center = (vertices[p[0]] + vertices[p[1]] + vertices[p[2]]) / 3;
			var norm = Vector3.Cross(vertices[p[1]] - vertices[p[0]], vertices[p[2]] - vertices[p[0]]) + center;
			
			List<Vector3>[] newVertices = {new List<Vector3>() {center}, new List<Vector3>() {center}};
			for (var i = 0; vertices.Count > i; i++) {
				if (i.Equals(p[0]) || i.Equals(p[1]) || i.Equals(p[2])) {
					newVertices[0].Add(vertices[i]);
					newVertices[1].Add(vertices[i]);
				}
				else if (Vector3.Dot(norm, vertices[i] - center) >= 0) {
					newVertices[0].Add(vertices[i]);
				}
				else {
					newVertices[1].Add(vertices[i]);
				}
			}
			
			var children = new List<Polygon>() {
				Factory.Create("Meteor Child", newVertices[0]),
				Factory.Create("Meteor Child", newVertices[1])
			};
			foreach (var c in children) {
				c.Rigidize();
				CopyFeature(c.gameObject);
			}
			Destroy(gameObject);
		}

		private int GetNearest(Vector3 p) {
			var distance = float.MaxValue;
			var nearest = 0;
			for (var i = 0; vertices.Count > i; i++) {
				var d = Vector3.Distance(p, vertices[i]);
				if (d < distance) {
					distance = d;
					nearest = i;
				}
			}
			return nearest;
		}

		private int GetFarthest(Vector3 p) {
			var distance = float.MinValue;
			var farthest = 0;
			for (var i = 0; vertices.Count > i; i++) {
				var d = Vector3.Distance(p, vertices[i]);
				if (d > distance) {
					distance = d;
					farthest = i;
				}
			}
			return farthest;
		}

		private void CopyFeature(GameObject target) {
			target.transform.position = transform.position;
			target.transform.rotation = transform.rotation;
			target.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
		}
		
	}

}
