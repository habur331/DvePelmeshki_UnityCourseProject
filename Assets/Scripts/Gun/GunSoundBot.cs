using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class GunSoundBot : MonoBehaviour
{
    [Header("Audio effects")]
    [SerializeField] private AudioClip shootSound;

    private Gun _gun;
    private AudioSource _audioSource;

    private void Start()
    {
        _gun = GetComponent<Gun>();
        _audioSource = GetComponent<AudioSource>();
        
        _gun.shootEvent.AddListener(OnShot);
    }

    private void OnShot() => _audioSource.PlayOneShot(shootSound);
}
