using UnityEngine;

namespace Code.Legacy
{
	public class LaserScript : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{

			Destroy(gameObject, 2f);

		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
			transform.Translate(Vector3.forward * 50);
		}
	}
}
