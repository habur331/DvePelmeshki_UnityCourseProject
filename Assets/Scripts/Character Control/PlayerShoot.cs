using System.Collections;
using System.Collections.Generic;
using Static_Classes;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float zoomInAim = 20;

    private GunSelector _gunSelector;

    private Camera _mainCamera = null;
    private float _normalZoom;
    
    public void Start()
    {
        _mainCamera = Camera.main;
        _normalZoom = _mainCamera!.fieldOfView;
        _gunSelector = GetComponentInChildren<GunSelector>();
    }
    
    public void Update()
    {
        Reload();
        Shoot();
        Aim();
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _gunSelector.CurrentGun.Reload();
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            _gunSelector.CurrentGun.Shoot(_mainCamera.transform);
        }
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
