using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Static_Classes;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class FPSInput : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float accelerationRate = 2;
    
    [Header("Jumping")]
    [SerializeField]
    private float jumpHeight = 2.0f;
    [SerializeField]
    private float jumpDuration = 1.0f;
    [SerializeField]
    private float gravity = 9f;

    private bool isJumping = false;
    
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var motion = transform.TransformDirection(Keyboard.Input()) * (speed * Time.deltaTime);

        if (Keyboard.IsShiftHeldDown())
        {
            motion.x *= accelerationRate;
            motion.z *= accelerationRate;
        }
        
        if (_characterController.isGrounded)
        {
            isJumping = false;
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                isJumping = true;
                Jump();
            }
        }
        
        motion.y -= gravity * Time.deltaTime;
        _characterController.Move(motion);
    }
    
    private void Jump()
    {
        var jumpTargetHeight = transform.position.y + jumpHeight;

        transform.DOMoveY(jumpTargetHeight, jumpDuration / 2).SetEase(Ease.OutQuad);
    }
}
