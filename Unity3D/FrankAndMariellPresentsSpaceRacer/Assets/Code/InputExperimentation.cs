using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class InputExperimentation : MonoBehaviour
    {
        private void Update()
        {
            var pressed = Keyboard.current.spaceKey.IsPressed();

            if (pressed)
            {
                // Debug.Log("SPAAAACE!");
            }
        }
    }
}