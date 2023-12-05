using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Image settingsPanel;
    
    private bool isGamePaused = false;
    private bool isSettingsPopupVisible = false;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        ToggleGamePause();
        ToggleSettingsPopup();
    }
    
    private void ToggleGamePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f; 
        Messenger<bool>.Broadcast(GameEvent.PauseStateChanged, isGamePaused);
    }

    private void ToggleSettingsPopup()
    {
        isSettingsPopupVisible = !isSettingsPopupVisible;
        settingsPanel.gameObject.SetActive(isSettingsPopupVisible);
    }
}
