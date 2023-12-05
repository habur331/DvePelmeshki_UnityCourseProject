using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursor : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Messenger<bool>.AddListener(GameEvent.PauseStateChanged, OnPauseToggle);
    }

    private void OnPauseToggle(bool isPaused)
    {
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        }
    }
}