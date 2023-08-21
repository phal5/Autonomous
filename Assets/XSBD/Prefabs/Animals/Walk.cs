using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;

public class Walk : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] float _runSpeed;
    [SerializeField] float _pace;
    [SerializeField] float _minPace;
    [SerializeField] float _height;
    [SerializeField] Transform _LFoot;
    [SerializeField] Transform _LHip;
    [SerializeField] Transform _RFoot;
    [SerializeField] Transform _RHip;
    RaycastHit _hit;
    Vector3 _stepPosition;
    float _paceDivisor;
    float _speedDivisor;
    [SerializeField] bool _LR;
    Rig _rig;
    // Start is called before the first frame update
    void Start()
    {
        _paceDivisor = 1 / _pace;
        _speedDivisor = 1 / _runSpeed;
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
            Step(_LFoot, _LHip, _RFoot, _RHip, PaceMultiplier());
        }
        else
        {
            Step(_RFoot, _RHip, _LFoot, _LHip, PaceMultiplier());
        }
    }

    void Step(Transform foot, Transform hip, Transform otherfoot, Transform otherHip, float paceMultiplier)
    {
        Vector3 paceData = Vector3.Scale(_stepPosition - otherHip.position, new Vector3(1, 0, 1));
        if (Cast(hip, paceData))
        {
            if (Vector3.SqrMagnitude(paceData) <= _pace * _pace * paceMultiplier)
            {
                foot.position = _hit.point;
                foot.position += Vector3.up
                    * (_pace * _pace - Vector3.SqrMagnitude(paceData))
                    * _paceDivisor * _paceDivisor * _height
                    * paceMultiplier;
                otherfoot.position = _stepPosition;
            }
            else
            {
                foot.position = _hit.point;
                SwitchFoot();
                _stepPosition = foot.position;
                if(Vector3.SqrMagnitude(_stepPosition - transform.position) > _pace * _pace * paceMultiplier)
                {
                    Physics.Raycast(hip.position, Vector3.down, out _hit);
                    _stepPosition = _hit.point;
                }
            }
        }
        else
        {
            Fall();
        }
    }

    bool Cast(Transform hip, Vector3 paceData)
    {
        return Physics.Raycast(new Ray(hip.position - Vector3.Dot(paceData, transform.forward) * transform.forward, Vector3.down), out _hit, 2);
    }

    float PaceMultiplier()
    {
        return _agent.velocity.sqrMagnitude * _speedDivisor * _speedDivisor;
    }

    void Fall()
    {
        
    }
}
