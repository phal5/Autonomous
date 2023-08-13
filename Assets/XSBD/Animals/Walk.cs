using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;

public class Walk : MonoBehaviour
{
    [SerializeField] float _pace;
    [SerializeField] float _height;
    [SerializeField] Transform _LFoot;
    [SerializeField] Transform _LHip;
    [SerializeField] Transform _RFoot;
    [SerializeField] Transform _RHip;
    RaycastHit _hit;
    Vector3 _stepPosition;
    float _paceDivisor;
    bool _LR;
    Rig _rig;
    // Start is called before the first frame update
    void Start()
    {
        _paceDivisor = 1 / _pace;
        TryGetComponent<Rig>(out _rig);
        _rig.weight = 0;
        _stepPosition = _LFoot.position;
        _rig.weight = 1;
    }

    // Update is called once per frame
    void Update()
    {
        WalkRun();
    }

    void SwitchFoot()
    {
        _LR ^= true;
    }

    void WalkRun()
    {
        if (_LR)
        {
            Step(_LFoot, _LHip, _RHip);
            _RFoot.position = _stepPosition;
        }
        else
        {
            Step(_RFoot, _RHip, _LHip);
            _LFoot.position = _stepPosition;
        }
    }

    void Step(Transform foot, Transform hip, Transform otherHip)
    {
        Vector3 paceData = Vector3.Scale(_stepPosition - otherHip.position, new Vector3(1, 0, 1));
        if (Cast(hip, paceData))
        {
            if (Vector3.SqrMagnitude(paceData) < _pace * _pace)
            {
                foot.position = _hit.point;
                foot.position += Vector3.up
                    * (_pace * _pace - Vector3.SqrMagnitude(paceData))
                    * _paceDivisor * _paceDivisor * _height;
            }
            else
            {
                SwitchFoot();
                _stepPosition = foot.position;
            }
        }
        else
        {
            Fall();
        }
    }

    bool Cast(Transform hip, Vector3 paceData)
    {
        return Physics.Raycast(new Ray(hip.position + paceData.sqrMagnitude * _paceDivisor * transform.forward, Vector3.down), out _hit, 2);
    }

    void Fall()
    {

    }
}
