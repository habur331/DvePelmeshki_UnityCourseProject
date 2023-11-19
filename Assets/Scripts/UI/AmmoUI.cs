using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    private GunSelector _gunSelector;
    private TMP_Text _text;

    private void Start()
    {
        _gunSelector = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<GunSelector>();
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text =
            $"{_gunSelector.CurrentGun.CurrentMagazineSize.ToString()}/{_gunSelector.CurrentGun.MagazineSize.ToString()}";
    }
}