using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bomb))]
[RequireComponent(typeof(AudioSource))]
public class BombSound : MonoBehaviour
{
    [SerializeField] private AudioClip bombPlantedClip;
    [SerializeField] private AudioClip bombTickClip;
    [SerializeField] private AudioClip bombBlowUpClip;

    private Bomb _bomb;
    private AudioSource _audioSource;

    private void Start()
    {
        _bomb = GetComponent<Bomb>();
        _audioSource = GetComponent<AudioSource>();

        _bomb.bombPlantedEvent.AddListener(() => _audioSource.PlayOneShot(bombPlantedClip));
        _bomb.bombTickEvent.AddListener(() => _audioSource.PlayOneShot(bombTickClip));
        _bomb.bombBlowUpEvent.AddListener(() => _audioSource.PlayOneShot(bombBlowUpClip));
    }
}