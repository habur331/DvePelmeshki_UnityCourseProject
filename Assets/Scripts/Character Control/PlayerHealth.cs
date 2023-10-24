using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerHealth : ReactiveTarget
{
    [SerializeField] private int health = 100;
    private int _currentHealth;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerSpawnPosition;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        playerSpawnPosition = player.transform.position;
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
        Debug.Log("You are dying");
        
        transform.DOMove(playerSpawnPosition, 0.000001f);
        _currentHealth = health;
    }

    public IEnumerator Falling()
    {
        Debug.Log("You are falling");

        var fallDuration = 3f;
        transform.DOMoveY(transform.position.y - 8, fallDuration);
        yield return new WaitForSeconds(fallDuration);

        Die();
    }
}