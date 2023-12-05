using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Bomb : MonoBehaviour, IInteractable
{
    [Header("Parameters")]
    [SerializeField] private int timeToDiffuse = 5;
    [SerializeField] private int timeToBlowUp = 45;
    
    [Header("Events")]
    [SerializeField] public UnityEvent bombDiffusedEvent;
    [SerializeField] public UnityEvent bombBlowUpEvent;
    [SerializeField] public UnityEvent bombTickEvent;

    public int TimeLeft => _timeLeftToBlowUp;
    
    [CanBeNull] private Coroutine _timerCoroutine;
    private bool IsDiffused => _timeDiffused >= timeToDiffuse;
    private bool _interactedLastFrame = false;
    private float _timeDiffused;
    private int _timeLeftToBlowUp;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        ResetIfNotInteracted();
        BlowUpIfTimeIsLeft();
    }

    public void Interact(GameObject responsiveUIElement = null)
    {
        _interactedLastFrame = true;
        // ReSharper disable once Unity.NoNullPropagation
        var slider = responsiveUIElement?.GetComponent<Slider>() ?? throw new InvalidOperationException();
        
        if (IsDiffused)
            return;
            
        _timeDiffused +=  Time.deltaTime;
        slider.value = _timeDiffused;
        slider.maxValue = timeToDiffuse;
        
       
        if (IsDiffused)
        {
            bombDiffusedEvent.Invoke();
        }
    }
    
    private void BlowUpIfTimeIsLeft()
    {
        if (_timeLeftToBlowUp <= 0)
        {
            bombBlowUpEvent.Invoke();
            StopCoroutine(_timerCoroutine);
        }
    }
    
    private void StartTimer()
    {
        _timeLeftToBlowUp = timeToBlowUp;
        _timerCoroutine = StartCoroutine(CoroutineHelper());
        return;
        
        IEnumerator CoroutineHelper()
        {
            for (;;)
            {
                yield return new WaitForSeconds(1);
                _timeLeftToBlowUp--;
                bombTickEvent.Invoke();
            }
        }
    }

    private void ResetIfNotInteracted()
    {
        if(!_interactedLastFrame)
            _timeDiffused = 0;
        _interactedLastFrame = false;
    }
}
