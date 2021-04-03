using UnityEngine;

namespace Code.Libraries.Dynagon {

	public class Background : MonoBehaviour {

		public Color background = new Color(0.05f, 0.05f, 0.1f);
		public Color ambient = new Color(0.5f, 0.5f, 0.5f);

		private void Start () {
			var skyMat = new Material(Shader.Find("Diffuse"));
			skyMat.color = background;
			RenderSettings.skybox = skyMat;
			RenderSettings.ambientLight = ambient;
		}

	}

}
