using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private TMP_Text _text;
    
    private void Start()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerHealth>();
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.SetText($"{_playerHealth.Health}");
    }
}
