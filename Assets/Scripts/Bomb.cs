using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour, IInteractable
{
    [SerializeField] private float timeToDiffuse = 5f;

    private float _timeDiffused;

    private bool IsDiffused => _timeDiffused >= timeToDiffuse;

    public void Interact(GameObject responsiveUIElement = null)
    {
        // ReSharper disable once Unity.NoNullPropagation
        var slider = responsiveUIElement?.GetComponent<Slider>() ?? throw new InvalidOperationException();
        
        if (IsDiffused)
            return;
            
        _timeDiffused +=  Time.deltaTime;
        slider.value = _timeDiffused;
        slider.maxValue = timeToDiffuse;

        if (IsDiffused)
        {
            Messenger.Broadcast(GameEvent.BombDiffused);
        }
    }
}
