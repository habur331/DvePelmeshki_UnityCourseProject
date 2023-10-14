using System;
using System.Collections;
using System.Collections.Generic;
using Static_Classes;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Serialization;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 15f;
    [FormerlySerializedAs("impactEffect")] [SerializeField] private GameObject bulletHole;
    [SerializeField] private ParticleSystem particleSystem;
    
    private Camera _mainCamera;
    private float _nextTimeToFire = 0f;
    
    public void Start()
    {
        _mainCamera = Camera.main;
    }

    public void Update()
    {
        if (Mouse.IsLeftButtonClicked() && Time.time >= _nextTimeToFire)
        {
            Debug.Log("shot");
            _nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        particleSystem.Play();
        var cameraTransform = _mainCamera.transform;
        
        var ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (!Physics.Raycast(ray, out var hit)) return;
        
        Debug.Log(hit.transform.name);
        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        
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
