using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bomb))]
[RequireComponent(typeof(AudioSource))]
public class BombSound : MonoBehaviour
{
    [SerializeField] private AudioClip bombBlowUpClip;
    [SerializeField] private AudioClip bombTickClip;
    
    private Bomb _bomb;
    private AudioSource _audioSource;
    
    private void Start()
    {
        _bomb = GetComponent<Bomb>();
        _audioSource = GetComponent<AudioSource>();
        
        _bomb.bombTickEvent.AddListener(() => { });
        _bomb.bombBlowUpEvent.AddListener(() => { });
    }
}
