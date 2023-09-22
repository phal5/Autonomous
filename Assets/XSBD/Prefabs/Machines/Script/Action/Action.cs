using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [SerializeField] protected Power _powerSupply;
    [SerializeField] protected float _speedMultiplier;

    protected float _Speed()
    {
        return _powerSupply.GetSpeed() * _speedMultiplier;
    }
}
