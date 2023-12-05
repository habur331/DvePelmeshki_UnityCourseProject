using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BotHealth))]
public class BotSound : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip headshotSound;

    private BotHealth _botHealth;
    private AudioSource _audioSource;

    private void Start()
    {
        _botHealth = GetComponent<BotHealth>();
        _audioSource = GetComponent<AudioSource>();

        _botHealth.bodyPartHitEvent.AddListener((bodyPart, _, _) => PlaySoundOnHit(bodyPart));
    }

    private void PlaySoundOnHit(BotBodyPartEnum bodyPart)
    {
        switch (bodyPart)
        {
            case BotBodyPartEnum.Head:
            {
                _audioSource.PlayOneShot(headshotSound);
                break;
            }

            case BotBodyPartEnum.Chest:
            case BotBodyPartEnum.Arm:
            case BotBodyPartEnum.Leg:
            default:
            {
                _audioSource.PlayOneShot(hitSound);
                break;
            }
        }
    }
}