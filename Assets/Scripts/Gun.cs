using System;
using System.Collections;
using System.Collections.Generic;
using Static_Classes;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class Gun : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private float zoomInAim = 20;
    private float _normalZoom;
    
    [Header("Effects")]
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private new ParticleSystem particleSystem;

    private Camera _mainCamera = null;
    private float _nextTimeToFire = 0f;
    
    public void Start()
    {
        _mainCamera = Camera.main;
        _normalZoom = _mainCamera!.fieldOfView;
    }

    public void Update()
    {
        if (Mouse.IsLeftButtonClicked() && Time.time >= _nextTimeToFire)
        {
            Debug.Log("shot");
            _nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
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

    private void Shoot()
    {
        particleSystem.Play();
        var cameraTransform = _mainCamera.transform;
        
        var ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (!Physics.Raycast(ray, out var hit)) return;
        
        Debug.Log(hit.transform.name);
        var mark = Instantiate(bulletHole, hit.point + (hit.normal * .01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
        mark.transform.parent = hit.transform;
        
        if (IsObjectReactiveTarget(hit.transform.gameObject, out var reactiveTarget))
        {
            reactiveTarget.ReactToHit();
        }
    }
    
    private bool IsObjectReactiveTarget(GameObject @object, out ReactiveTarget reactiveTarget)
    {
        reactiveTarget = @object.GetComponent<ReactiveTarget>();
        return reactiveTarget is not null;
    }
}
