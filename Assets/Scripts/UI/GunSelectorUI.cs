using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelectorUI : MonoBehaviour
{
    private GunSelector _gunSelector;
    
    private void Start()
    {
        _gunSelector = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<GunSelector>();
    }
}
