using Static_Classes;
using UnityEngine;

public class MouseLookX : MonoBehaviour
{
    public float cameraRotateXMin;
    public float cameraRotateXMax;
    
    public float mouseSmooth;

    private float _xRotation; 

    private void Start()
    {
        _xRotation = transform.localRotation.x;
    }

    private void Update()
    {
        RotateCameraX(Mouse.AxisInput(mouseSmooth).y);
    }
    
    private void RotateCameraX(float mouseYInput)
    {
        _xRotation += mouseYInput;
        _xRotation = Mathf.Clamp(_xRotation, cameraRotateXMin, cameraRotateXMax);
        transform.localRotation = Quaternion.Euler(-_xRotation, 0f, 0f);
    }
}
