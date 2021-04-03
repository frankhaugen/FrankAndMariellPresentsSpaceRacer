using UnityEngine;

namespace Code.Libraries.Dynagon {

	public class Meteors : MonoBehaviour {

		public GameObject meteor;
		public GameObject spawnPoint;
		public float speed = 1f;
		public float interval = 0.5f;

		private float timer = 0f;

		private void Create() {
			var polygon = Instantiate(
				meteor,
				spawnPoint.transform.position,
				Quaternion.identity
				) as GameObject;
			polygon.GetComponent<Rigidbody>().velocity = (Vector3.zero - polygon.transform.position) * speed;
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
