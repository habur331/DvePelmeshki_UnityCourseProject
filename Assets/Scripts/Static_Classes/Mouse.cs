using UnityEngine;
using Input = UnityEngine.Windows.Input;

namespace Static_Classes
{
    public static class Mouse
    {
        public static Vector2 AxisInput(float mouseSmooth)
        {
            var mouseXInput = UnityEngine.Input.GetAxisRaw("Mouse X") * mouseSmooth;
            var mouseYInput = UnityEngine.Input.GetAxisRaw("Mouse Y") * mouseSmooth;

            return new Vector2(mouseXInput, mouseYInput);
        }

        public static bool IsLeftButtonClicked() => UnityEngine.Input.GetMouseButtonDown(0);
        
        public static bool IsRightButtonClicked() => UnityEngine.Input.GetMouseButtonDown(1);
    }
}
