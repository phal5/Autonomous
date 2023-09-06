using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;

public class AdvancedWalk : MonoBehaviour
{
    [System.Serializable] class FootPair
    {
        [SerializeField] Transform _foot1;
        [SerializeField] Transform _hip1;
        [Space(10f)]
        [SerializeField] Transform _foot2;
        [SerializeField] Transform _hip2;
        Vector3 _footPosition;
        bool _12;

        bool Cast(Transform foot, Transform hip, Vector3 offset, float distance, out RaycastHit hit)
        {
            return Physics.Raycast(hip.position + offset, Vector3.down, out hit, distance);
        }

        float SqrFlatVector(Vector3 a, Vector3 b)
        {
            return Vector3.Scale(Vector3.up + Vector3.right, a - b).sqrMagnitude;
        }

        bool Step(Transform foot, Transform hip, Transform fixedFoot, Transform fixedHip, float pace)
        {
            if (SqrFlatVector(foot.position, hip.position) <= pace)
            {
                //go
                return true;
            }
            else
            {
                //switch foot
                return false;
            }
        }
    }
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] float _runSpeed;
    [SerializeField] float _pace;
    [SerializeField] float _minPaceTime = 0.2f;
    [SerializeField] float _height;
    [SerializeField] FootPair[] _footPairs;

    RaycastHit _hit;
    Vector3 _stepPosition1;
    Vector3 _stepPosition2;
    float _paceDivisor;
    float _speedDivisor;
    float _paceTimer;
    bool _switchFoot;
    [SerializeField] bool _LR;
    Rig _rig;
    // Start is called before the first frame update
    void Start()
    {
        _paceDivisor = 1 / _pace;
        _speedDivisor = 1 / _runSpeed;
        TryGetComponent<Rig>(out _rig);
        _rig.weight = 0;
        _rig.weight = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float SqrPaceMultiplier()
    {
        return _agent.velocity.sqrMagnitude * _speedDivisor * _speedDivisor;
    }
}
