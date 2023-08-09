using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Peck : MonoBehaviour
{
    [SerializeField] Rig _chainIK;
    [SerializeField] Behaviour _wrapUp;
    
    float _timer;
    bool _peck;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _timer = 0;
        _peck = false;
    }

    private void OnDisable()
    {
        _wrapUp.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        SetMovement();
        Feed();
    }

    void SetMovement()
    {
        _timer += Time.deltaTime;
        if (_peck)
        {
            if (_timer > 1)
            {
                _timer = -0.5f;
                _peck = false;
            }
        }
        else
        {
            if (_timer > 0.5f)
            {
                _timer = 0.5f - (int)(Random.value * 3) * 0.25f;
                _peck = true;
            }
        }
    }

    void Feed()
    {
        if (_peck)
        {
            float timer = _timer * 4 - (int)(_timer * 4) - 0.5f;
            _chainIK.weight = timer * timer + 0.75f;
        }
        else
        {
            if (Abs(_timer) > 0.25f)
            {
                float timer = (_timer > 0) ? _timer  - 0.25f : 0.25f + _timer;
                _chainIK.weight = timer * timer * 16;
            }
            else
            {
                _chainIK.weight = 0;
            }
        }
    }

    float Abs(float _float)
    {
        return (_float > 0) ? _float : -_float;
    }
}
