using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Bot))]
public class BotHealth : MonoBehaviour
{
    [SerializeField] private float health = 100;

    [Space]
    [SerializeField] private float headDamageMultiplier = 1f;
    [SerializeField] private float chestDamageMultiplier = 1f;
    [SerializeField] private float armDamageMultiplier = 1f;
    [SerializeField] private float legDamageMultiplier = 1f;

    [HideInInspector] public UnityEvent<BotBodyPartEnum, float, Vector3> bodyPartHit;
    
    private float _currentHealth;

    private Bot _bot;
    private Animator _animator;
    private IReadOnlyCollection<BotBodyPart> _bodyParts;
    private IReadOnlyCollection<Rigidbody> _rigidbodies;
    private IReadOnlyCollection<Collider> _colliders;
    private IReadOnlyDictionary<BotBodyPartEnum, float> _damageMultipliers;

    private void Start()
    {
        _currentHealth = health;
        _bot = GetComponent<Bot>();
        _animator = GetComponent<Animator>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _bodyParts = GetComponentsInChildren<BotBodyPart>();
        _colliders = GetComponentsInChildren<Collider>();
        
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
        
        foreach (var botBodyPart in _bodyParts)
        {
            botBodyPart.hitEvent.AddListener(ReactToHit);
        }
        
        _damageMultipliers = new Dictionary<BotBodyPartEnum, float>()
        {
            {BotBodyPartEnum.Head, headDamageMultiplier},
            {BotBodyPartEnum.Chest, chestDamageMultiplier},
            {BotBodyPartEnum.Arm, armDamageMultiplier},
            {BotBodyPartEnum.Leg, legDamageMultiplier},
        };
    }

    private void ReactToHit(BotBodyPartEnum bodyPartEnum, int damage = 0)
    {
        if (damage < 0)
            throw new InvalidOperationException();

        var takenDamage = damage * _damageMultipliers[bodyPartEnum];
        _currentHealth -= takenDamage;
        
        bodyPartHit.Invoke(bodyPartEnum, takenDamage , this.transform.position);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        this.enabled = false;
        _bot.StopAllCoroutines();
        _bot.enabled = false;
        Destroy(_bot.CurrentGun.gameObject);
        
        _animator.enabled = false;
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
        foreach (var botBodyPart in _bodyParts)
        {
            botBodyPart.enabled = false;
        }

        /*foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }*/
    }
}