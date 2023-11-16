using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Bot : MonoBehaviour
{
    [SerializeField]
    private bool shootingEnable = true;
    [SerializeField]
    private GameObject gunShootPoint;
    [SerializeField]
    private Vector3 playerOffset;

    [SerializeField]
    private float delayBeforeShooting = 0.1f; 
    [SerializeField]
    private int shotsInBurst = 3;
    [SerializeField]
    private float timeBetweenShots = 0.5f;

    public Gun CurrentGun => _currentGun;
    
    private Gun _currentGun;
    private GameObject _player;
    
    private bool _isShooting = false;
    private bool CanShoot => shootingEnable && !_currentGun.IsReloading;

    private void Start()
    {
        _currentGun = GetComponentInChildren<Gun>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (CanShoot)
        {
            if (IsPlayerInFront())
            {
                if (!_isShooting)
                {
                    // Start shooting after the delay
                    _isShooting = true;
                    Invoke(nameof(StartShooting), delayBeforeShooting);
                }
            }
            else
            {
                // Stop shooting if the player is not in front
                //isShooting = false;
                //CancelInvoke(nameof(StartShooting));
            }
        }

        transform.DODynamicLookAt(_player.transform.position + playerOffset, 0.5f).SetEase(Ease.Linear);
    }

    private void StartShooting()
    {
        StartCoroutine(ShootAtPlayer());
    }

    private IEnumerator ShootAtPlayer()
    {
        for (var i = 0; i < shotsInBurst; i++)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            if(_currentGun != null)
                _currentGun.Shoot(gunShootPoint.transform);
        }

        _isShooting = false;
    }

    private bool IsPlayerInFront()
    {
        var ray = new Ray(gunShootPoint.transform.position, gunShootPoint.transform.forward);
        return Physics.Raycast(ray, out var hit) && hit.collider.CompareTag("Player");
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
        this.DOKill();
    }

    private void OnDisable()
    {
        StopCoroutine(ShootAtPlayer());
        StopAllCoroutines();
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(gunShootPoint.transform.position, gunShootPoint.transform.forward * 50);
    }*/
}
