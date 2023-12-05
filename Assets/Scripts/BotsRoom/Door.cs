using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform transferPosition;
    
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Interact(GameObject responsiveUIElement = null)
    {
        TransferPlayer();
    }

    private void TransferPlayer()
    {
        _player.transform.DOMove(transferPosition.position, 0.0001f);
    }
}
