using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BotHealth : ReactiveTarget
{
    [SerializeField] private int health = 100;
    private int _currentHealth;

    private Bot _bot;

    private void Start()
    {
        _bot = GetComponent<Bot>();
        _currentHealth = health;
    }

    public override void ReactToHit(int damage = 0)
    {
        if (damage < 0)
            throw new InvalidOperationException();

        _currentHealth -= damage;

        if (_currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}