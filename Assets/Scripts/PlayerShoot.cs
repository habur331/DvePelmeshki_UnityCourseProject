using System.Collections;
using System.Collections.Generic;
using Static_Classes;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float zoomInAim = 20;

    private Camera _mainCamera = null;
    private float _normalZoom;
    private Gun _currentGun = null;
    
    public void Start()
    {
        _mainCamera = Camera.main;
        _normalZoom = _mainCamera!.fieldOfView;
        _currentGun = GetComponentInChildren<Gun>();
    }
    
    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _currentGun.Shoot(_mainCamera.transform);
        }
        
        Aim();
    }
    
    private void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _mainCamera.fieldOfView = zoomInAim;
        }

        if (Input.GetMouseButtonUp(1))
        {
            _mainCamera.fieldOfView = _normalZoom;
        }
    }
}
