using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class GunAnimation : MonoBehaviour
{
    [SerializeField] private float punchForce = 0.1f;
    [SerializeField] private float shootAnimationDuration = 0.1f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float elasticity = 1f;
    
    private Gun _gun;

    private void Start()
    {
        _gun = GetComponent<Gun>();
        _gun.shootEvent.AddListener(OnShoot);
    }

    private void OnShoot()
    {
        transform.DOKill(true);
        transform.DOPunchPosition(Vector3.back * punchForce, shootAnimationDuration, vibrato, elasticity);
    }
    
    private void OnDisable()
    {
        transform.DOKill(true);
    }
}
