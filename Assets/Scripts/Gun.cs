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
    [SerializeField] private int damage = 10;
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
    
    public void Shoot(Transform originTransform)
    {
        if (Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / fireRate;
            
            particleSystem.Play();

            var ray = new Ray(originTransform.position, originTransform.forward);
            if (!Physics.Raycast(ray, out var hit)) return;

            if (!hit.collider.CompareTag("Player"))
            {
                var mark = Instantiate(bulletHole, hit.point + (hit.normal * .01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                mark.transform.parent = hit.transform;
            }
        
            if (IsObjectReactiveTarget(hit.transform.gameObject, out var reactiveTarget))
            {
                reactiveTarget.ReactToHit(damage);
            }
        }
    }
    
    private bool IsObjectReactiveTarget(GameObject @object, out ReactiveTarget reactiveTarget)
    {
        reactiveTarget = @object.GetComponent<ReactiveTarget>();
        return reactiveTarget is not null;
    }
}
