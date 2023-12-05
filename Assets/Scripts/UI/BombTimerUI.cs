using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BombTimerUI : MonoBehaviour
{
    private TMP_Text _text;
    [CanBeNull] private Bomb _bomb; 
    
    private void Start()
    {
        _text = GetComponentInChildren<TMP_Text>(includeInactive: true);
        Messenger<GameObject>.AddListener(GameEvent.BombPlanted, bombGameObject =>
        {
            _bomb = bombGameObject.GetComponentInChildren<Bomb>();
        });
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (_bomb != null)
        {
            if(!_text.gameObject.activeSelf)
                _text.gameObject.SetActive(true);
            
            _text.SetText(_bomb.TimeLeft.ToString());
        }
        else
        {
            if(_text.gameObject.activeSelf)
                _text.gameObject.SetActive(false);
        }
    }
}
