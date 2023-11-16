using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BotBodyPart : MonoBehaviour, IReactiveTarget
{
    [SerializeField] private BotBodyPartEnum bodyPart;
    
    [HideInInspector] public UnityEvent<BotBodyPartEnum, int> hit;

    public void ReactToHit(int damage = 0)
    {
       hit.Invoke(bodyPart, damage);
    }
}
