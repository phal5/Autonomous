using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Flap : MonoBehaviour
{
    [SerializeField] Rig _rig;
    [SerializeField] Behaviour _wrapUp;
    float _timer = 0;
    bool _loop = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_loop)
        {
            Loop();
        }
        else
        {
            Begin();
        }
    }

    private void OnDisable()
    {
        _loop = false;
        _wrapUp.enabled = true;
    }

    void Loop()
    {
        _rig.weight = (0.75f - Mathf.Cos(_timer * Mathf.PI) * 0.25f);
        _timer += Time.deltaTime * 9;
        if (_timer > 2)
        {
            _timer = 0;
        }
    }

    void Begin()
    {
        if(_timer < 1)
        {
            _timer += Time.deltaTime * 9;
            _rig.weight = _timer;
        }
        else
        {
            _loop = true;
        }
    }
}
