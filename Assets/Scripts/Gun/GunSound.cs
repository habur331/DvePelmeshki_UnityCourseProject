using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
[RequireComponent(typeof(GunAnimation))]
[RequireComponent(typeof(AudioSource))]
public class GunSound : MonoBehaviour
{
    [Header("Audio effects")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip takeGunInHandSound;
    [SerializeField] private AudioClip startReloadSound;
    [SerializeField] private AudioClip endReloadSound;

    private Gun _gun;
    private GunAnimation _gunAnimation;
    private AudioSource _audioSource;

    private void Start()
    {
        _gun = GetComponent<Gun>();
        _gunAnimation = GetComponent<GunAnimation>();
        _audioSource = GetComponent<AudioSource>();
        
        _gun.shootEvent.AddListener(OnShot);
        _gun.startReloadingEvent.AddListener(OnStartReloading);
        _gun.stopReloadingEvent.AddListener(OnStopReloading);
        _gunAnimation.gunIsTakenInHand.AddListener(OnGunTakeInHand);
    }

    private void OnShot() => _audioSource.PlayOneShot(shootSound);

    private void OnGunTakeInHand() => _audioSource.PlayOneShot(takeGunInHandSound);

    private void OnStartReloading() => _audioSource.PlayOneShot(startReloadSound, 2.5f);

    private void OnStopReloading() => _audioSource.PlayOneShot(endReloadSound);
}