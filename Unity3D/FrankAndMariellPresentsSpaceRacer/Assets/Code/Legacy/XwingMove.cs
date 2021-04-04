using UnityEngine;

namespace Code.Legacy
{
	public class XwingMove : MonoBehaviour {

		public Vector3 velocity;

		Rigidbody xwing;
		public Vector3 speed;
		float speedMultiplier = 2f;
		float rotationMultiplier = 3f;

		// Use this for initialization
		void Start ()
		{
			xwing = GetComponent<Rigidbody>();
		}
	
		// Update is called once per frame
		void Update ()
		{

		}

		// Update is called once per frame
		void FixedUpdate ()
		{
			velocity = xwing.velocity;
		}

		public void Move (float moveValues, Vector3 rotateValues)
		{
			speed.x = 0f;
			speed.y = 0f;
			speed.z = 1f + moveValues * speedMultiplier;

			//xwing.transform.Rotate(rotateValues * speedMultiplier * moveValues) ;

			xwing.AddRelativeTorque(rotateValues * rotationMultiplier);
			//xwing.transform.Translate(speed);


			xwing.AddRelativeForce(new Vector3(0f, 0f, moveValues * speedMultiplier));
			//xwing. = xwing.rotation.eulerAngles * moveValues * speedMultiplier;


			//Vector3 AddPos = ;
			//AddPos = xwing.rotation * AddPos;
			//xwing.velocity = rotateValues + Vector3.forward * moveValues * Mathf.Pow(speedMultiplier, 5f);

		}
	}
}
