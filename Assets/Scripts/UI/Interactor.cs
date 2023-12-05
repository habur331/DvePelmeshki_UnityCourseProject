using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

interface IInteractable
{
    public void Interact([CanBeNull] GameObject responsiveUIElement = null);
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactRange;
    [SerializeField] private Image interactButton;
    [SerializeField] private Slider bombDiffusionProgressSlider;
    
    private Transform _mainCameraTransform;
    private Dictionary<Type, GameObject> _interactElements = new();

    private void Start()
    {
        _mainCameraTransform = Camera.main!.transform;
        _interactElements[typeof(Bomb)] = bombDiffusionProgressSlider.gameObject;
    }

    private void Update()
    {
        interactButton.gameObject.SetActive(false);
        foreach (var interactElement in _interactElements.Values)
        {
            interactElement.SetActive(false);
        }
        
        var ray = new Ray(_mainCameraTransform.position, _mainCameraTransform.forward);
        if (!Physics.Raycast(ray, out var hitInfo, interactRange, ~LayerMask.GetMask("Player"))) return;

        if (!hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) return;
        
        if (Input.GetKey(KeyCode.E))
        {
            interactButton.gameObject.SetActive(false);
            if (_interactElements.TryGetValue(interactObj.GetType(), out var interactElement))
            {
                interactElement.SetActive(true);
            }
            interactObj.Interact(interactElement);
        }
        else
        {
            interactButton.gameObject.SetActive(true);
        }
    }
}