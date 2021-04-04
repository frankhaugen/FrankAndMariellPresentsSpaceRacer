using UnityEngine;

namespace Code.Legacy
{
	public class Movement : MonoBehaviour
	{
		public GameObject prefab;

		public float speedMultiplier;
		public float speed;
		public float rotationMultiplier;

		// input variables'

		public float inputPitch;
		public float inputYaw;
		public float inputRoll;

		Vector3 direction;

		// Use this for initialization
		void Start ()
		{
			speedMultiplier = 50f;
			rotationMultiplier = 1f;

		
		}
	
		// Update is called once per frame
		void Update ()
		{
		
			if(Input.GetKeyDown(KeyCode.Space))
			{
				//Instantiate(prefab, transform.position + transform.forward * 5, transform.rotation);
			}
		}

	

		// Update is called once per frame
		void FixedUpdate()
		{
			transform.Translate(Vector3.forward * speedMultiplier);

			UpdateFunctions();
		
		}

		void UpdateFunctions()
		{
			UpdateInput();
			//if(CheckInput()) { Rotate(); }
		}

		void MoveTranslate()
		{

		}

		bool CheckInput()
		{
			if (inputPitch != 0f)
			{
				return true;
			}
			if (inputRoll != 0f)
			{
				return true;
			}
			if (inputYaw != 0f)
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}

		void Rotate()
		{
			//direction.x = inputPitch * rotationMultiplier;
			//direction.y = inputYaw * rotationMultiplier;
			//direction.z = inputRoll * rotationMultiplier / 2f;

			direction.x = inputPitch;
			direction.y = inputYaw;
			direction.z = inputRoll;

			transform.Rotate(direction);
		
		}

		void UpdateInput()
		{
			inputPitch = Input.GetAxisRaw("Pitch");
			inputRoll = Input.GetAxisRaw("Roll");
			inputYaw = Input.GetAxisRaw("Yaw");

		}
	}
}
