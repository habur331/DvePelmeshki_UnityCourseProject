using System.Collections;
using System.Collections.Generic;
using Static_Classes;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private new Camera camera;

    [SerializeField] private float cameraRotateXMin;
    [SerializeField] private float cameraRotateXMax;

    [SerializeField] private float mouseSmooth;

    private float _xRotation;
    private Vector2 _recoil = Vector2.zero;

    private void Start()
    {
        _xRotation = camera.transform.localRotation.x;
    }

    private void Update()
    {
        Rotate(Mouse.AxisInput(mouseSmooth) + _recoil);
        _recoil = Vector2.zero;;
    }

    private void Rotate(Vector2 input)
    {
        player.transform.Rotate(Vector3.up * input.x);
        _xRotation += input.y;
        _xRotation = Mathf.Clamp(_xRotation, cameraRotateXMin, cameraRotateXMax);
        camera.transform.localRotation = Quaternion.Euler(-_xRotation, 0f, 0f);
    }

    public void ApplyRecoil(Vector2 recoil)
    {
        _recoil = recoil;
    }
}