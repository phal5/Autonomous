using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Peck : MonoBehaviour
{
    [SerializeField] Rig _chainIK;
    [SerializeField] float _peckRange;
    [SerializeField] Transform _target;
    [SerializeField] Behaviour _wrapUp;

    float _timer;
    bool _peck;
    Animals _animals;
    
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Animals>(out _animals);
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
        if (_animals.GetIsEating())
        {
            SetTarget();
            MovementTimer();
            Feed();
        }
        else
        {
            WrapUp();
        }
    }

    void MovementTimer()
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

    void SetTarget()
    {
        _target.position = _animals.GetFoodTarget();
    }

    void WrapUp()
    {
        if (_chainIK.weight > 0)
        {
            _chainIK.weight -= Time.deltaTime * 8;
            _timer = 0;
            _peck = false;
        }
    }
}
