using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Static_Classes;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private float zoomInAim = 20;

    [Space]
    [Header("Recoil")]
    [SerializeField] private bool enableRecoil = false;
    [SerializeField] private GameObject shootingCharacter;
    
    [Space]
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;

    [Space]
    [SerializeField] private float aimRecoilX;
    [SerializeField] private float aimRecoilY;

    [Space]
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;
    
    [Space]
    [Header("Reloading")]
    [SerializeField] private int magazineSize = 5;
    [SerializeField] private float timeToReload = 2f;
    
    [Space]
    [Header("Visual effects")]
    [SerializeField] [CanBeNull] private GameObject bulletMark;
    [SerializeField] [CanBeNull] private new ParticleSystem particleSystem;
    [SerializeField] [CanBeNull] private new AudioSource audioSource;

    [Space]
    [Header("Audio effects")]
    [SerializeField] [CanBeNull] private AudioClip shootSound;
    [SerializeField] [CanBeNull] private AudioClip startReloadSound;
    [SerializeField] [CanBeNull] private AudioClip endReloadSound;

    [DoNotSerialize] public UnityEvent reloadingEvent;
    
    public int CurrentMagazineSize { get; protected set; }
    public int MagazineSize => magazineSize;
    public bool IsReloading => _reloading;
    
    private Camera _mainCamera = null;
    private float _nextTimeToFire = 0f;
    private float _normalZoom;
    private bool _reloading = false;
    
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    


    private bool MustReload => CurrentMagazineSize == 0;
    private bool CanShoot => Time.time >= _nextTimeToFire && !MustReload && !_reloading;
    
    private void Start()
    {
        if(audioSource is null)
            TryGetComponent(out audioSource);
        
        _mainCamera = Camera.main;
        _normalZoom = _mainCamera!.fieldOfView;
        CurrentMagazineSize = magazineSize;
    }

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        _mainCamera.gameObject.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Shoot(Transform originTransform)
    {
        if (MustReload)
        {
            Reload();
        }
        
        if (CanShoot)
        {
            _nextTimeToFire = Time.time + 1f / fireRate;
            CurrentMagazineSize--;
            
            // ReSharper disable once Unity.NoNullPropagation
            particleSystem?.Play();
            // ReSharper disable once Unity.NoNullPropagation
            if(shootSound != null) audioSource?.PlayOneShot(shootSound);

            var ray = new Ray(originTransform.position, originTransform.forward);
            if (!Physics.Raycast(ray, out var hit)) return;

            PlaceBulletMark(hit);
        
            if (IsObjectReactiveTarget(hit.transform.gameObject, out var reactiveTarget))
            {
                reactiveTarget.ReactToHit(damage);
            }
            
            ApplyRecoil();
        }
    }
    
    private void ApplyRecoil()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), currentRotation.z);
    }

    public void Reload()
    {
        if (_reloading) return;
        
        reloadingEvent.Invoke();
        StartCoroutine(ReloadCoroutine());
    }
    
    [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
    private IEnumerator ReloadCoroutine()
    {
        _reloading = true;
        _nextTimeToFire = Time.time + timeToReload;
        
        yield return null;
        if(startReloadSound != null) audioSource?.PlayOneShot(startReloadSound, 2.5f);

        yield return new WaitForSeconds(timeToReload);

        if (endReloadSound != null)
        {
            audioSource?.PlayOneShot(endReloadSound);
            yield return new WaitForSeconds(endReloadSound.length);
        }
        
        CurrentMagazineSize = magazineSize;
        _reloading = false;
    }

    private void PlaceBulletMark(RaycastHit hit)
    {
        if(ReferenceEquals(bulletMark, null)) return;
            
        if (hit.collider.CompareTag("Player")) return;
        
        var mark = Instantiate(bulletMark, hit.point + (hit.normal * .01f), Quaternion.FromToRotation(Vector3.up, hit.normal));
        mark.transform.parent = hit.transform;
    }

    private bool IsObjectReactiveTarget(GameObject @object, out ReactiveTarget reactiveTarget)
    {
        reactiveTarget = @object.GetComponent<ReactiveTarget>();
        return reactiveTarget is not null;
    }
}
