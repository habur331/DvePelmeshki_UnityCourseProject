using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BotsNumberSelectorUI : MonoBehaviour
{
    [SerializeField] private int botsNumber = 5;

    private TMP_Text _text;

    private void OnEnable()
    {
        if (_text is not null)
            _text.SetText(botsNumber.ToString());
    }

    private void Start()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _text.SetText(botsNumber.ToString());
    }

    public void OnButtonClicked(int value)
    {
        botsNumber += value;
        botsNumber = Math.Max(0, botsNumber);
        _text.SetText(botsNumber.ToString());
        Messenger<int>.Broadcast(UIEvent.BotsNumberInSettingsChanged, botsNumber);
    }
}