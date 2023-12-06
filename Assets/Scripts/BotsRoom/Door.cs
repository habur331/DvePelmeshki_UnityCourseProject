using System;
using System.Collections;
using System.Collections.Generic;
using Character_Control;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum DoorType
{   
    Default,
    Enter,
    Exit
}

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform transferPosition;
    [SerializeField] private DoorType type;

    [HideInInspector] public UnityEvent playerTransferred;

    public DoorType Type => type;
    
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
        _player.GetComponent<Player>().TransferTo(transferPosition.position);
        
        playerTransferred.Invoke();
    }
}
