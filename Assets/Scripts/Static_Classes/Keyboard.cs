using UnityEngine;

namespace Static_Classes
{
    public class Keyboard
    {
        public static Vector3 Input()
        {
            var xInput = UnityEngine.Input.GetAxis("Horizontal");
            var zInput = UnityEngine.Input.GetAxis("Vertical");

            return new Vector3(xInput, 0, zInput);
        }

        public static bool IsShiftHeldDown()
        {
            return UnityEngine.Input.GetKey(KeyCode.LeftShift);
        }
    }
}