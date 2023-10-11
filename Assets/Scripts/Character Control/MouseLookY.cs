using System.Collections;
using System.Collections.Generic;
using Static_Classes;
using UnityEngine;

public class MouseLookY : MonoBehaviour
{
    public float mouseSmooth;
    
    private void Update()
    {
        RotatePlayY(Mouse.AxisInput(mouseSmooth).x);
    }
    
    private void RotatePlayY(float mouseXInput)
    {
        transform.Rotate(Vector3.up * mouseXInput);
    }
}
