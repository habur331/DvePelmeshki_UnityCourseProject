using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Static_Classes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = System.Random;

public class Gun : MonoBehaviour
{
    #region Inspector

    [Header("Shooting")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private LayerMask notTargetLayers;

    [Space]
    [Header("Recoil")]
    [SerializeField] private bool randomRecoil = false;
    
    [Header("Random Recoil")]
    [SerializeField] private Vector2 randomRecoilBoundaries;
    
    [Header("Pattern Recoil")]
    [SerializeField] private float timeToResetRecoil = 0.5f;
    [SerializeField] private Vector2[] recoilPattern = { Vector2.zero };

    [Space]
    [Header("Reloading")]
    [SerializeField] private int magazineSize = 5;
    [SerializeField] private float timeToReload = 2f;

    [Space]
    [Header("Visual effects")]
    [SerializeField] [CanBeNull] private GameObject bulletMark;
    [SerializeField] [CanBeNull] private new ParticleSystem particleSystem;

    #endregion


    #region Events

    [HideInInspector] public UnityEvent reloadingEvent;
    [HideInInspector] public UnityEvent startReloadingEvent;
    [HideInInspector] public UnityEvent stopReloadingEvent;
    [HideInInspector] public UnityEvent shootEvent;

    #endregion

    
    #region Public State

    public bool IsActive { get; set; } = true;
    public int CurrentMagazineSize { get; protected set; }
    public int MagazineSize => magazineSize;
    public bool IsReloading => _reloading;

    #endregion
    
    
    #region Private state

    private Random _random;
    private Camera _mainCamera = null;
    private MouseLook _mouseLook;
    private float _nextTimeToFire = 0f;
    private float _normalZoom;
    private bool _reloading = false;

    private RecoilEnumerator _recoilEnumerator;
    private bool _shotLastFrame = false;
    [CanBeNull] private Coroutine _recoilResetCoroutine = null;

    private bool MustReload => CurrentMagazineSize == 0;
    private bool CanShoot => Time.time >= _nextTimeToFire && !MustReload && !_reloading && IsActive;

    #endregion

    
    private void Start()
    {
        _mainCamera = Camera.main;
        _mouseLook = _mainCamera!.GetComponent<MouseLook>();
        _normalZoom = _mainCamera!.fieldOfView;
        CurrentMagazineSize = magazineSize;
        
        if(!randomRecoil)
            _recoilEnumerator = new RecoilEnumerator(recoilPattern);
    }

    private void Update()
    {
        if (!randomRecoil)
        {
            if (_shotLastFrame && _recoilResetCoroutine is null && _recoilEnumerator.Started)
                _recoilResetCoroutine = StartCoroutine(ResetRecoil());

            if (_shotLastFrame && _recoilResetCoroutine is not null)
            {
                StopCoroutine(_recoilResetCoroutine);
                _recoilResetCoroutine = StartCoroutine(ResetRecoil());
            }

            _shotLastFrame = false;
        }
    }
    
    private void OnDisable()
    {
        _reloading = false;
    }

    public void Shoot(Transform originTransform)
    {
        if (MustReload)
        {
            Reload();
        }

        if (CanShoot)
        {
            shootEvent.Invoke();
            
            _nextTimeToFire = Time.time + 1f / fireRate;
            CurrentMagazineSize--;

            // ReSharper disable once Unity.NoNullPropagation
            particleSystem?.Play();

            var direction = originTransform.forward;
            if (randomRecoil)
                direction = Quaternion.Euler(GetRandomRecoil()) * direction;
            
            var ray = new Ray(originTransform.position, direction);
            if (!Physics.Raycast(ray, out var hit, float.MaxValue, ~notTargetLayers, QueryTriggerInteraction.Ignore)) return;

            PlaceBulletMark(hit);

            if (IsObjectReactiveTarget(hit.transform.gameObject, out var reactiveTarget))
            {
                reactiveTarget.ReactToHit(damage);
            }

            ApplyPatternRecoil();
            _shotLastFrame = true;
        }
    }

    private void ApplyPatternRecoil()
    {
        if (randomRecoil) return;
        
        _recoilEnumerator.MoveNext();
        _mouseLook.ApplyRecoil(_recoilEnumerator.Current);
    }

    private Vector3 GetRandomRecoil()
    {
        var randomX = UnityEngine.Random.Range(-randomRecoilBoundaries.x, randomRecoilBoundaries.x);
        var randomY = UnityEngine.Random.Range(-randomRecoilBoundaries.y, randomRecoilBoundaries.y);
        return new Vector3(randomX, randomY, 0);
    }

    private IEnumerator ResetRecoil()
    {
        yield return new WaitForSeconds(timeToResetRecoil);
        _recoilEnumerator.Reset();
        _recoilResetCoroutine = null;
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

        yield return new WaitForNextFrameUnit();
        startReloadingEvent.Invoke();

        yield return new WaitForSeconds(timeToReload);
        stopReloadingEvent.Invoke();
        CurrentMagazineSize = magazineSize;
        _reloading = false;
    }

    private void PlaceBulletMark(RaycastHit hit)
    {
        if (ReferenceEquals(bulletMark, null)) return;

        if (hit.collider.CompareTag("Player")) return;
        if (hit.collider.CompareTag("HumanoidEnemy")) return;

        var mark = Instantiate(bulletMark, hit.point + (hit.normal * .01f),
            Quaternion.FromToRotation(Vector3.up, hit.normal));
        mark.transform.parent = hit.transform;
    }

    private bool IsObjectReactiveTarget(GameObject @object, out IReactiveTarget reactiveTarget)
    {
        reactiveTarget = @object.GetComponent<IReactiveTarget>();
        return reactiveTarget is not null;
    }
}