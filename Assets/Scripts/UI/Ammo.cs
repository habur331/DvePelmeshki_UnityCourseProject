using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private PlayerShoot _playerShoot;
    private TMP_Text _text;

    private void Start()
    {
        _playerShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text =
            $"{_playerShoot.CurrentGun.CurrentMagazineSize.ToString()}/{_playerShoot.CurrentGun.MagazineSize.ToString()}";
    }
}