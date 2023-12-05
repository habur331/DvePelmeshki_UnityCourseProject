using System;
using System.Collections;
using System.Collections.Generic;
using Character_Control;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Player))]
public class PlayerHealth : MonoBehaviour, IReactiveTarget
{
    [SerializeField] private int health = 100; 
    [SerializeField] private Vector3 playerSpawnPosition;

    public int Health => _currentHealth;
    
    private int _currentHealth;
    private Player _player;
    private GameObject _playerGameObject;
    

    void Start()
    {
        _playerGameObject = GameObject.FindGameObjectsWithTag("Player")[0];
        playerSpawnPosition = _playerGameObject.transform.position;
        _currentHealth = health;
    }

    public void ReactToHit(int damage = 0)
    {
        if (damage < 0)
            throw new InvalidOperationException();

        _currentHealth -= damage;

        if (_currentHealth <= 0)
            _player.Die();
    }

    public void RestoreHealth()
    {
        _currentHealth = health;
    }

    /*public IEnumerator Falling()
    {
        Debug.Log("You are falling");

        var fallDuration = 3f;
        transform.DOMoveY(transform.position.y - 8, fallDuration);
        yield return new WaitForSeconds(fallDuration);

        Die();
    }*/
}