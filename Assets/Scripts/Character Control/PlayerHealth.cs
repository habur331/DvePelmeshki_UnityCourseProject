using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerHealth : ReactiveTarget
{
    [SerializeField] private int health;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerSpawnPosition;
    void Start()
    {
        health = 100;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        playerSpawnPosition = player.transform.position;
    }
    
    void Update()
    {
       
    }

    public override void ReactToHit(int damage = 0)
    {
        health -= damage;
        
        if (health < 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("You are dying");
       
        player.transform.position = playerSpawnPosition;
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
