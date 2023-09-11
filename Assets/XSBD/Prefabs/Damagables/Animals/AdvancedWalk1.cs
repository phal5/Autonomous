using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;

public class AdvancedWalk1 : MonoBehaviour
{
    [System.Serializable] class FootPair
    {

        [SerializeField] Transform _foot1;
        [SerializeField] Transform _hip1;
        [Space(10f)]
        [SerializeField] Transform _foot2;
        [SerializeField] Transform _hip2;
        RaycastHit _hit;
        Vector3 _stepPosition;
        float _paceDivisor;
        bool _12;
        
        //Settings-----------------------------------------------------------------------------------------------------
        public void SetPaceDivisor(float paceDivisor)
        {
            _paceDivisor = paceDivisor;
        }

        //Calculators--------------------------------------------------------------------------------------------------
        Vector3 Horizontal(Vector3 vector3)
        {
            return Vector3.Scale(Vector3.forward + Vector3.right, vector3);
        }

        //Actors-------------------------------------------------------------------------------------------------------
        private bool Cast(Transform hip, Vector3 offset)
        {
            return Physics.Raycast(hip.position + offset, Vector3.down, out _hit, 10);
        }

        private void Step(Transform foot, Transform hip, Transform fixedFoot, Vector3 fixedHip, Vector3 forward, bool timeUp, float doubleSqrPace, float height, ref bool switchFeet)
        {
            Vector3 horizontal = Horizontal(fixedHip - fixedFoot.position);
            float sqrMagnitude = horizontal.sqrMagnitude;
            if(Cast(hip, Vector3.Dot(horizontal, forward) * forward))
            {
                float f = (sqrMagnitude * sqrMagnitude < doubleSqrPace) ? doubleSqrPace - sqrMagnitude * sqrMagnitude : 0;
                if (timeUp && f == 0)
                {
                    switchFeet = true;
                }
                else
                {
                    foot.position = _hit.point;
                    foot.position += Vector3.up
                    * f * height * _paceDivisor * _paceDivisor * _paceDivisor;
                    fixedFoot.position = _stepPosition;
                    Debug.Log(_hit.point.y);
                }
            }
            else
            {
                //fall
            }
        }

        public void Walk(Vector3 forward, bool timeUp, float doubleSqrPace, float height, ref bool switchFeet)
        {
            if (_12)
            {
                Step(_foot1, _hip1, _foot2, _hip2.position, forward, timeUp, doubleSqrPace, height, ref switchFeet);
            }
            else
            {
                Step(_foot2, _hip2, _foot1, _hip1.position, forward, timeUp, doubleSqrPace, height, ref switchFeet);
            }
        }

        public void SwitchFeet()
        {
            if (_12)
            {
                _stepPosition = _foot1.position;
            }
            else
            {
                _stepPosition = _foot2.position;
            }
            _12 ^= true;
        }
    }

    [SerializeField] NavMeshAgent _agent;
    [SerializeField] FootPair[] _footPairs;

    [Tooltip("Pace when Speed = 1")]
    [SerializeField] float _minimumPace;
    [SerializeField] float _standardPace;
    [SerializeField] float _minPaceTime = 0.2f;
    [SerializeField] float _height;
    Vector3 _prevPos;
    [SerializeField] float _timer;
    float _doubleSqrPace;
    [SerializeField] bool _switchFeet;
    
    // Start is called before the first frame update
    void Start()
    {
        float _paceDivisor = 1 / _standardPace;
        Debug.Log(_paceDivisor);
        foreach(FootPair footPair in _footPairs)
        {
            footPair.SetPaceDivisor(_paceDivisor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetDoubleSqrPace();
        _timer += Time.deltaTime;
        foreach (FootPair footPair in _footPairs)
        {
            footPair.Walk(
                transform.forward,
                _timer > _minPaceTime && Vector3.SqrMagnitude(_prevPos - transform.position) > _minimumPace * _minimumPace
                , _doubleSqrPace, _height,
                ref _switchFeet
            );
        }
        if (_switchFeet)
        {
            foreach (FootPair footPair in _footPairs)
            {
                footPair.SwitchFeet();
            }
            _prevPos = transform.position;
            _timer = 0;
            _switchFeet = false;
        }
    }

    void SetDoubleSqrPace()
    {
        _doubleSqrPace = _agent.velocity.sqrMagnitude * _standardPace * _standardPace * _standardPace * _standardPace;
    }
}
