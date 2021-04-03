using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Libraries.Dynagon {

	public static class Triangulator3D {
		
		private static Tetrahedron GetHugeTetrahedron(List<Vector3> vertices) {
			var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			foreach (var v in vertices) {
				max = Vector3.Max(max, v);
				min = Vector3.Min(min, v);
			}
			var c = (max + min) * 0.5f;
			var radius  = Vector3.Distance(c, min) + 0.1f;
			return new Tetrahedron(
				new Vector3(c.x, c.y + 3.0f * radius, c.z),
				new Vector3(c.x - 2.0f * Mathf.Sqrt(2.0f) * radius, c.y - radius, c.z),
				new Vector3(c.x + Mathf.Sqrt(2.0f) * radius, c.y - radius, c.z + Mathf.Sqrt(6.0f) * radius),
				new Vector3(c.x + Mathf.Sqrt(2.0f) * radius, c.y - radius, c.z - Mathf.Sqrt(6.0f) * radius)
				);
		}
		
		private static float Lu(ref float[][] a, ref int[] ip) {
			const int n = 3;
			var weight = new float[n];
			
			for (var k = 0; k < n; ++k) {
				ip[k] = k;
				float u = 0;
				for(var j = 0; j < n; ++j) {
					var t = Mathf.Abs(a[k][j]);
					if (t > u) {
						u = t;
					}
				}
				if (u == 0) {
					return 0;
				}
				weight[k] = 1 / u;
			}
			float det = 1;
			for (var k = 0; k < n; ++k) {
				float u = -1;
				var m = 0;
				for (var i = k; i < n; ++i) {
					var ii = ip[i];
					var t = Mathf.Abs(a[ii][k]) * weight[ii];
					if(t > u) {
						u = t;
						m = i;
					}
				}
				var ik = ip[m];
				if (m != k) {
					ip[m] = ip[k];
					ip[k] = ik;
					det = -det;
				}
				u = a[ik][k];
				det *= u;
				if (u == 0) {
					return 0;
				}
				for (var i = k+1; i < n; ++i) {
					var ii = ip[i];
					var t = (a[ii][k] /= u);
					for (var j = k+1; j < n; ++j) {
						a[ii][j] -= t * a[ik][j];
					}
				}
			}
			return det;
		}
		
		private static void Solve(float[][] a, float[] b, int[] ip, ref float[] x) {
			const int n = 3;
			
			for (var i = 0; i < n; ++i) {
				var ii = ip[i];
				var t = b[ii];
				for (var j = 0; j < i; ++j) {
					t -= a[ii][j] * x[j];
				}
				x[i] = t;
			}
			
			for (var i = n-1; i >= 0; --i) {
				var t = x[i];
				var ii = ip[i];
				for (var j = i+1; j < n; ++j) {
					t -= a[ii][j] * x[j];
				}
				x[i] = t / a[ii][i];
			}
		}
		
		private static float Gauss(float[][] a, float[] b, ref float[] x) {
			var ip = new int[3];
			var det = Lu(ref a, ref ip);
			
			if (det != 0) {
				Solve(a, b, ip, ref x);
			}
			return det;
		}
		
		private static Sphere GetCircumsphere(Tetrahedron tetra) {
			var p = tetra.p;
			
			var a = new float[][] {
				new float[] {p[1].x - p[0].x, p[1].y - p[0].y, p[1].z - p[0].z}, 
				new float[] {p[2].x - p[0].x, p[2].y - p[0].y, p[2].z - p[0].z},
				new float[] {p[3].x - p[0].x, p[3].y - p[0].y, p[3].z - p[0].z} 
			};
			
			float[] b = {
				0.5f * (p[1].x * p[1].x - p[0].x * p[0].x + p[1].y * p[1].y - p[0].y * p[0].y + p[1].z * p[1].z - p[0].z * p[0].z),
				0.5f * (p[2].x * p[2].x - p[0].x * p[0].x + p[2].y * p[2].y - p[0].y * p[0].y + p[2].z * p[2].z - p[0].z * p[0].z),
				0.5f * (p[3].x * p[3].x - p[0].x * p[0].x + p[3].y * p[3].y - p[0].y * p[0].y + p[3].z * p[3].z - p[0].z * p[0].z)
			};
			
			float[] x = {0f, 0f, 0f};
			
			var det = Gauss(a, b, ref x);
			if (det == 0) {
				return new Sphere(Vector3.zero, -1);
			}
			else {
				var center = new Vector3(x[0], x[1], x[2]);
				return new Sphere(center, Vector3.Distance(center, p[0]));
			}
		}
		
		private static List<Triangle> GetDelaunayTriangles(List<Vector3> vertices) {
			var tetras = new HashSet<Tetrahedron>();
			
			var huge = GetHugeTetrahedron(vertices);
			tetras.Add(huge);
			
			foreach (var v in vertices) {
				var counter = new Counter<Tetrahedron>();
				var trash = new List<Tetrahedron>();
				
				foreach (var t in tetras) {
					var sphere = GetCircumsphere(t);
					if (Vector3.Distance(sphere.center, v) < sphere.radius) {
						counter.Add(new Tetrahedron(v, t.p[0], t.p[1], t.p[2]));
						counter.Add(new Tetrahedron(v, t.p[0], t.p[2], t.p[3]));
						counter.Add(new Tetrahedron(v, t.p[0], t.p[1], t.p[3]));
						counter.Add(new Tetrahedron(v, t.p[1], t.p[2], t.p[3]));
						trash.Add(t);
					}
				}
				tetras.RemoveWhere(t => trash.Contains(t));
				
				foreach (KeyValuePair<Tetrahedron, int> entry in counter) {
					if (entry.Value == 1) {
						tetras.Add(entry.Key);
					}
				}
			}

			tetras.RemoveWhere(t => huge.ShareVertex(t));
			
			var triangles = new HashSet<Triangle>();
			foreach (var t in tetras) {
				triangles.Add(new Triangle(t.p[0], t.p[1], t.p[2]));
				triangles.Add(new Triangle(t.p[0], t.p[2], t.p[3]));
				triangles.Add(new Triangle(t.p[0], t.p[1], t.p[3]));
				triangles.Add(new Triangle(t.p[1], t.p[2], t.p[3]));
			}
			return triangles.ToList();
		}

		public static List<Vector3> Triangulate(List<Vector3> vertices) {
			return Function.ConvertTrianglesToList(GetDelaunayTriangles(vertices));
		}
		
	}
}
