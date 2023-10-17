using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Bot : MonoBehaviour
{
    [SerializeField] 
    private GameObject gunShootPoint;
    [SerializeField] 
    private Vector3 playerOffset;

    private Gun _currentGun;
    private GameObject _player;
    

    private void Start()
    {
        _currentGun = GetComponentInChildren<Gun>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        ShootAtPlayer();
        transform.DODynamicLookAt(_player.transform.position + playerOffset, 0.5f).SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        this.DOKill();
    }

    private void ShootAtPlayer()
    {
        if (IsPlayerInFront())
        {
            _currentGun.Shoot(gunShootPoint.transform);
        }
    }

    private bool IsPlayerInFront()
    {
        var ray = new Ray(gunShootPoint.transform.position, gunShootPoint.transform.forward);
        return Physics.Raycast(ray, out var hit) && hit.collider.CompareTag("Player");;
    }
    
    /*private void OnDrawGizmos()
   {
       Gizmos.color = Color.red;
       Gizmos.DrawLine(gunShootPoint.transform.position, gunShootPoint.transform.forward * 50);
   }*/
}