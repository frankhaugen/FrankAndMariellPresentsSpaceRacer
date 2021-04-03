using UnityEngine;

namespace Code.Libraries.Dynagon {

	public class Orbit : MonoBehaviour {

		public Vector3 origin = Vector3.zero;
		public Vector3 target = Vector3.zero;
		public float speed = 30f;

		private void Update () {
			var p = transform.position - origin;
			transform.position = (Quaternion.Euler(0, Time.deltaTime * speed, 0) * p) + origin;
			transform.LookAt(target);
		}
	}

}
