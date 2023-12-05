using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using TMPro;
using UnityEngine;

public class LevelSelectorUI : MonoBehaviour
{
    private TMP_Text _text;

    [SerializeField] private int level = 1;

    private void OnEnable()
    {
        if (_text is not null)
            _text.SetText(level.ToString());
    }
    
    private void Start()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _text.SetText(level.ToString());
        Messenger<int>.AddListener(UIEvent.AimRoomLevelChanged, OnLevelChanged);
    }

    public void OnButtonClicked(int value)
    {
        level += value;
        level = Math.Max(1, level);
        _text.SetText(level.ToString());
        Messenger<int>.Broadcast(UIEvent.AimRoomLevelInSettingsChanged, level);
    }

    private void OnLevelChanged(int level)
    {
        this.level = level;
    }
}