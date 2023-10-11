using System;
using System.Collections;
using System.Collections.Generic;
using Static_Classes;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class FPSInput : MonoBehaviour
{
    public float speed;
    public float accelerationRate;
    
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var motion = transform.TransformDirection(Keyboard.Input()) * (speed * Time.deltaTime);
        
        if (Keyboard.IsShiftHeldDown())
            motion *= accelerationRate;

        _characterController.Move(motion);
    }
}
