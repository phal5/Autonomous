using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;

public class AdvancedWalk : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] float _runSpeed;
    [SerializeField] float _pace;
    [SerializeField] float _minPaceTime = 0.2f;
    [SerializeField] float _height;
    [SerializeField] Transform _LFoot1;
    [SerializeField] Transform _LHip1;
    [SerializeField] Transform _RFoot1;
    [SerializeField] Transform _RHip1;
    [SerializeField] Transform _LFoot2;
    [SerializeField] Transform _LHip2;
    [SerializeField] Transform _RFoot2;
    [SerializeField] Transform _RHip2;

    RaycastHit _hit;
    Vector3 _stepPosition1;
    Vector3 _stepPosition2;
    float _paceDivisor;
    float _speedDivisor;
    float _paceTimer;
    bool _switchFoot;
    bool _LR;
    Rig _rig;
    // Start is called before the first frame update
    void Start()
    {
        _paceDivisor = 1 / _pace;
        _speedDivisor = 1 / _runSpeed;
        TryGetComponent<Rig>(out _rig);
        _rig.weight = 0;
        _stepPosition1 = _LFoot1.position;
        _stepPosition2 = _RFoot2.position;
        _rig.weight = 1;
    }

    // Update is called once per frame
    void Update()
    {
        _switchFoot = SwitchFoot();
        WalkRun(_LFoot1, _LHip1, _RFoot1, _RHip1, _stepPosition1);
        WalkRun(_LFoot2, _LHip2, _RFoot2, _RHip2, _stepPosition2);
        _paceTimer += Time.deltaTime;
    }

    bool SwitchFoot()
    {
        if(_paceTimer > _minPaceTime)
        {
            _LR ^= true;
            _paceTimer = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    void WalkRun(Transform LFoot, Transform LHip, Transform RFoot, Transform RHip, Vector3 stepPosition)
    {
        
        if (_LR)
        {
            Step(LFoot, LHip, RFoot, RHip, SqrPaceMultiplier(), ref stepPosition);
        }
        else
        {
            Step(RFoot, RHip, LFoot, LHip, SqrPaceMultiplier(), ref stepPosition);
        }
    }

    void Step(Transform foot, Transform hip, Transform otherfoot, Transform otherHip, float paceMultiplier, ref Vector3 stepPosition)
    {
        Vector3 paceData = Vector3.Scale(stepPosition - otherHip.position, new Vector3(1, 0, 1));
        
        if (Vector3.SqrMagnitude(paceData) <= _pace * _pace * paceMultiplier)
        {
            if (Cast(hip, paceData))
            {
                foot.position = _hit.point;
                foot.position += Vector3.up
                    * (_pace * _pace - Vector3.SqrMagnitude(paceData))
                    * _paceDivisor * _paceDivisor * _height
                    * paceMultiplier;
                otherfoot.position = stepPosition;
            }
            else
            {
                Fall();
            }
        }
        else
        {
            if (_switchFoot)
            {
                Cast(hip, -transform.forward * _pace * PaceMultiplier());
                foot.position = _hit.point;

                stepPosition = foot.position;
                if (Vector3.SqrMagnitude(stepPosition - transform.position) > _pace * _pace * paceMultiplier)
                {
                    Physics.Raycast(hip.position, Vector3.down, out _hit);
                    stepPosition = _hit.point;
                }
            }
        }
    }

    bool Cast(Transform hip, Vector3 paceData)
    {
        return Physics.Raycast(new Ray(hip.position - Vector3.Dot(paceData, transform.forward) * transform.forward, Vector3.down), out _hit, 10);
    }

    float SqrPaceMultiplier()
    {
        return _agent.velocity.sqrMagnitude * _speedDivisor * _speedDivisor;
    }

    //heavy, minimize use
    float PaceMultiplier()
    {
        return _agent.velocity.magnitude * _speedDivisor;
    }

    void Fall()
    {
        
    }
}
