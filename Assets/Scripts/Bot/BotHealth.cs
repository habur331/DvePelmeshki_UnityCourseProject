using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BotHealth : MonoBehaviour
{
    [SerializeField] private float health = 100;

    [Space]
    [SerializeField] private float headDamageMultiplier = 1f;
    [SerializeField] private float chestDamageMultiplier = 1f;
    [SerializeField] private float armDamageMultiplier = 1f;
    [SerializeField] private float legDamageMultiplier = 1f;
    
    private float _currentHealth;

    private Bot _bot;
    private IReadOnlyCollection<BotBodyPart> _bodyParts;
    private IReadOnlyDictionary<BotBodyPartEnum, float> _damageMultipliers;

    private void Start()
    {
        _bot = GetComponent<Bot>();
        _currentHealth = health;
        _bodyParts = GetComponentsInChildren<BotBodyPart>();
        foreach (var botBodyPart in _bodyParts)
        {
            botBodyPart.hit.AddListener(ReactToHit);
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

        _currentHealth -= damage * _damageMultipliers[bodyPartEnum];

        if (_currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}