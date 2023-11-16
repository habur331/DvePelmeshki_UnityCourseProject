using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactiveTarget
{
    public void ReactToHit(int damage = 0);
}