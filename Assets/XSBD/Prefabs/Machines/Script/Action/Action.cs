using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [SerializeField] Power _powerSupply;
    [SerializeField] float _speedMultiplier;

    protected float _Speed()
    {
        return _powerSupply.GetSpeed() * _speedMultiplier;
    }
}
