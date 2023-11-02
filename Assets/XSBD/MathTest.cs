using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTest : MonoBehaviour
{
    [SerializeField] bool _faster;
    [SerializeField] int _iteration = 1000000000;
    [SerializeField] float _calculate;
    [SerializeField] float _result;

    // Update is called once per frame
    void Update()
    {
        Calc10000Times();
    }

    void Calc10000Times()
    {
        if (!_faster)
        {
            for(int i = 0; i < _iteration; ++i)
            {
                _result = 1 / (float)Math.Sqrt(_calculate);
            }
        }
        else
        {
            for(int i = 0; i < _iteration; ++i)
            {
                _result = CustomMath.QRsqrt(_calculate);
            }
        }
    }
}
