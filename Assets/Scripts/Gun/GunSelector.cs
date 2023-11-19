using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunSelector : MonoBehaviour
{
    [SerializeField] private Gun[] weapon;
    [SerializeField] private int selectedGunIndex;

    public Gun CurrentGun => weapon[selectedGunIndex];
    private void Start()
    {
        if (weapon.Length == 0)
        {
            enabled = false;
            return;
        }

        for (var i = 0; i < weapon.Length; i++)
        {
            weapon[i].gameObject.SetActive(selectedGunIndex == i);
        }
    }

    private void Update()
    {
        ChangeGun(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void ChangeGun(float scrollWheelInput)
    {
        if (Mathf.Abs(scrollWheelInput) < 0.01f)
            return;
        
        var direction = scrollWheelInput > 0 ? 1 : -1;
        weapon[selectedGunIndex].gameObject.SetActive(false);
        selectedGunIndex = (selectedGunIndex + direction + weapon.Length) % weapon.Length;
        weapon[selectedGunIndex].gameObject.SetActive(true);
    }
}