using UnityEngine;

namespace Code.Libraries.Dynagon {

	public class Illumination : MonoBehaviour {

		public Color color = new Color(0.3f, 0.4f, 0.85f);
		public float intensity = 1f;

		private void Start () {
			GetComponent<Light>().color = color;
			GetComponent<Light>().intensity = intensity;
		}

	}

}
