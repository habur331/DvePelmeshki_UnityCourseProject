using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventSystem;
using TMPro;
using UnityEngine;

public class InformationPopup : MonoBehaviour
{
    [SerializeField] private float showUpTime = 1f;

    private TMP_Text _text;
    private Coroutine _coroutine;

    private void Start()
    {
        _text = GetComponentInChildren<TMP_Text>(includeInactive: true);

        Messenger<int>.AddListener(UIEvent.AimRoomLevelChanged, OnLevelChange);

        Messenger.AddListener(GameEvent.PlayerDied, () => DisplayTextForDuration("You died"));

        Messenger.AddListener(GameEvent.BombDiffused, () => DisplayTextForDuration("Bomb diffused"));

        Messenger<GameObject>.AddListener(GameEvent.BombPlanted, _ => DisplayTextForDuration("Bomb has been planted"));

        Messenger.AddListener(GameEvent.BombBlewUp, () => DisplayTextForDuration("Bomb has blew up"));
    }

    private void OnLevelChange(int level)
    {
        if (AimingRoom.AimingRoom.IsPlayerIn)
        {
            DisplayTextForDuration($"Level {level}");
        }
    }


    private void DisplayTextForDuration(string text)
    {
        if (_coroutine is not null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(CoroutineHelper());
        return;

        IEnumerator CoroutineHelper()
        {
            _text.gameObject.SetActive(true);
            _text.SetText(text);

            yield return new WaitForSeconds(showUpTime);

            _text.gameObject.SetActive(false);
        }
    }
}