using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Gun))]
public class GunAnimation : MonoBehaviour
{
    [Header("Taking in hand animation parameters")]
    [SerializeField] private int disableAngle = 30;

    [SerializeField] private float timeToTakeInHand = 0.7f;

    [Header("Shoot animation parameters")]
    [SerializeField] private float punchForce = 0.1f;

    [SerializeField] private float shootAnimationDuration = 0.1f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float elasticity = 1f;

    [HideInInspector] public UnityEvent weaponIsTakenInHand;

    private Gun _gun;

    private Vector3 _activeRotation;
    private Vector3 _inactiveRotation;

    [CanBeNull] private Tweener _punchTweener;
    [CanBeNull] private Tweener _rotateTweener;

    private static bool _isFirstEnable = true;
    [CanBeNull] private static Action _doOnStart;

    private void OnEnable()
    {
        _activeRotation = transform.localRotation.eulerAngles;
        _inactiveRotation = _activeRotation + new Vector3(disableAngle, 0);

        if (!_isFirstEnable)
        {
            transform.localRotation = Quaternion.Euler(_inactiveRotation);

            _rotateTweener = transform.DOLocalRotateQuaternion(Quaternion.Euler(_activeRotation), timeToTakeInHand)
                .OnStart(() =>
                {
                    if (_gun is not null)
                    {
                        _gun.IsActive = false;
                    }
                    else
                    {
                        _doOnStart = () =>
                        {
                            _gun.IsActive = false;
                            _doOnStart = null;
                        };
                    }
                })
                .OnComplete(() =>
                {
                    weaponIsTakenInHand.Invoke();
                    _gun.IsActive = true;
                })
                .OnKill(() => transform.localRotation = Quaternion.Euler(_activeRotation));
        }

        _isFirstEnable = false;
    }

    private void Start()
    {
        _gun = GetComponent<Gun>();
        _gun.shootEvent.AddListener(OnShoot);
        _doOnStart?.Invoke();
    }

    private void OnShoot()
    {
        transform.DOKill(true);
        _punchTweener =
            transform.DOPunchPosition(Vector3.back * punchForce, shootAnimationDuration, vibrato, elasticity);
    }

    private void OnDisable()
    {
        _punchTweener?.Kill(true);
        _rotateTweener?.Kill();

        _punchTweener = null;
        _rotateTweener = null;
    }
}