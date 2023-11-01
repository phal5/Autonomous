using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class PlayerWalk : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _runSpeed;
    [SerializeField] float _pace;
    [SerializeField] float _minPaceTime = 0.2f;
    [SerializeField] float _height;
    [SerializeField] float _maxHeight = 0.5f;
    [SerializeField] float _headBob = 0.5f;
    [Space(10f)]
    [SerializeField] Transform _LFootTarget;
    [SerializeField] Transform _LHip;
    [SerializeField] Transform _RFootTarget;
    [SerializeField] Transform _RHip;
    [Space(10f)]
    [SerializeField] Transform _root;

    RaycastHit _hit;
    Vector3 _stepPosition;
    Vector3 _initialRootPosition;
    Vector3 _rootBuffer;
    Vector3 _leftFootBuffer;
    Vector3 _rightFootBuffer;
    Vector3 _rootVelocity;
    Vector3 _leftFootVelocity;
    Vector3 _rightFootVelocity;
    float _paceDivisor;
    float _speedDivisor;
    float _paceTimer;
    bool _LR;
    Rig _rig;

    // Start is called before the first frame update
    void Start()
    {
        _paceDivisor = 1 / _pace;
        _speedDivisor = 1 / _runSpeed;
        TryGetComponent<Rig>(out _rig);
        _rig.weight = 0;
        _stepPosition = _LFootTarget.position;
        _rig.weight = 1;
        _rootBuffer = _initialRootPosition = _root.localPosition;
        _leftFootBuffer = _LFootTarget.position;
        _rightFootBuffer = _RFootTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        WalkRun();
        _paceTimer += Time.deltaTime;
    }

    bool SwitchFoot()
    {
        if (_paceTimer > _minPaceTime)
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

    void WalkRun()
    {

        if (_LR)
        {
            Step(ref _leftFootBuffer, _LHip, ref _rightFootBuffer, _RHip, SqrPaceMultiplier());
        }
        else
        {
            Step(ref _rightFootBuffer, _RHip, ref _leftFootBuffer, _LHip, SqrPaceMultiplier());
        }
        _RFootTarget.position = Vector3.SmoothDamp(_RFootTarget.position, _rightFootBuffer, ref _rightFootVelocity, 0.1f);
        _LFootTarget.position = Vector3.SmoothDamp(_LFootTarget.position, _leftFootBuffer, ref _leftFootVelocity, 0.1f);
        _root.localPosition = Vector3.SmoothDamp(_root.localPosition, _rootBuffer, ref _rootVelocity, 0.1f);
    }

    void Step(ref Vector3 foot, Transform hip, ref Vector3 otherfoot, Transform otherHip, float paceMultiplier)
    {
        Vector3 paceData = Vector3.Scale(_stepPosition - otherHip.position, new Vector3(1, 0, 1));

        if (Vector3.SqrMagnitude(paceData) <= _pace * _pace * paceMultiplier)
        {
            if (Cast(hip.position, paceData + Vector3.up * 0.5f))
            {
                foot = _hit.point;
                float bump = (_pace * _pace * paceMultiplier - Vector3.SqrMagnitude(paceData))
                    * _paceDivisor * _paceDivisor * _height
                    * paceMultiplier;
                    
                if(bump > _maxHeight)
                {
                    bump = _maxHeight;
                }

                foot += bump * Vector3.up;
                Physics.Raycast(_stepPosition + Vector3.up, Vector3.down, out _hit);
                if (_hit.collider.gameObject.layer == 4) Physics.Raycast(_hit.point, Vector3.down, out _hit);
                otherfoot = _hit.point;
                _rootBuffer = _initialRootPosition + Vector3.up * (_headBob * bump);
            }
            else
            {
                Fall(ref foot, hip);
            }
        }
        else
        {
            if (SwitchFoot())
            {
                if (Cast(hip.position, -transform.forward * _pace * PaceMultiplier()))
                {
                    foot = _hit.point;
                    _stepPosition = foot;
                }
                else Fall(ref foot, hip);

                if (Vector3.SqrMagnitude(_stepPosition - transform.position) > _pace * _pace * paceMultiplier)
                {
                    Physics.Raycast(hip.position, Vector3.down, out _hit);
                    if (_hit.collider.gameObject.layer == 4) Physics.Raycast(_hit.point, Vector3.down, out _hit);
                    _stepPosition = _hit.point;
                }
            }
        }
    }

    bool Cast(Vector3 origin, Vector3 paceData)
    {
        return (PaceCastOnce(origin, paceData) ? (_hit.collider.gameObject.layer == 4) ? Physics.Raycast(_hit.point, Vector3.down, out _hit) : true : false);
    }

    bool PaceCastOnce(Vector3 origin, Vector3 paceData)
    {
        return Physics.Raycast(origin - Vector3.Dot(paceData, transform.forward) * transform.forward, Vector3.down, out _hit);
    }

    float SqrPaceMultiplier()
    {
        return _rigidbody.velocity.sqrMagnitude * _speedDivisor * _speedDivisor;
    }
    
    //heavy, minimize use
    float PaceMultiplier()
    {
        return _rigidbody.velocity.magnitude * _speedDivisor;
    }
    
    void Fall(ref Vector3 foot, Transform hip)
    {
        foot = _stepPosition = hip.position - Vector3.up;
        _rootBuffer = _initialRootPosition;
    }
}
