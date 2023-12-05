using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BotHealth))]
public class DamagePopupGenerator : MonoBehaviour
{
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private float showTime = 1f;

    private BotHealth _botHealth;
    private Dictionary<BotBodyPartEnum, Color> _textColorOnHit;

    private void Start()
    {
        _botHealth = GetComponent<BotHealth>();
        _botHealth.bodyPartHitEvent.AddListener((bodyPart, damage, position) =>
            CreatePopup(bodyPart, damage, position + new Vector3(0, 1.5f) ));

        _textColorOnHit = new Dictionary<BotBodyPartEnum, Color>()
        {
            { BotBodyPartEnum.Head, Color.red },
            { BotBodyPartEnum.Chest, Color.yellow },
            { BotBodyPartEnum.Arm, Color.gray },
            { BotBodyPartEnum.Leg, Color.gray }
        };
    }

    private void CreatePopup(BotBodyPartEnum bodyPart, float damage, Vector3 position)
    {
        var popup = Instantiate(damagePopup, position, Quaternion.identity);
        var text = popup.GetComponentInChildren<TMP_Text>();
        
        text.SetText(damage.ToString(CultureInfo.InvariantCulture));
        text.color = _textColorOnHit[bodyPart];

        Destroy(popup, showTime);
    }
}