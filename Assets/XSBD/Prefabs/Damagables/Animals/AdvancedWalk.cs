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
        RaycastHit _hit;
        static float _paceDivisor;
        static float _height;
        static bool _12;

        public static void SetPaceDivisor(float paceDivisor)
        {
            _paceDivisor = paceDivisor;
        }
        
        public static void SetHeight(float height)
        {
            _height = height;
        }

        public void Walk(float pace, bool switched)
        {
            if (switched)
            {
                _12 ^= true;
            }
            if (_12)
            {
                Step(_foot1, _hip1, _foot2.position, _hip2.position, pace, _height);
            }
            else
            {
                Step(_foot2, _hip2, _foot1.position, _hip1.position, pace, _height);
            }
        }

        bool Cast(Transform foot, Transform hip, Vector3 offset, out RaycastHit hit, float distance = 10)
        {
            return Physics.Raycast(hip.position + offset, Vector3.down, out hit, distance);
        }

        Vector3 Horizontal(Vector3 a, Vector3 b)
        {
            return Vector3.Scale(Vector3.up + Vector3.right, a - b);
        }

        bool Step(Transform foot, Transform hip, Vector3 fixedFoot, Vector3 fixedHip, float sqrPace, float height)
        {
            Vector3 step = Horizontal(fixedHip, fixedFoot);
            float footData = step.sqrMagnitude;
            if (Cast(foot, hip, step, out _hit))
            {
                foot.position = _hit.point;
                foot.position += Vector3.up
                    * (sqrPace - footData)
                    * _paceDivisor * _paceDivisor
                    * height;
                return true;
            }
            else
            {
                //Falling
                return false;
            }
        }
    }

    [SerializeField] NavMeshAgent _agent;
    [SerializeField] FootPair[] _footPairs;

    [Tooltip("Pace when Speed = 1")]
    [SerializeField] float _pace;
    [SerializeField] float _minPaceTime = 0.2f;
    [SerializeField] float _height;

    Vector3 _pivot;
    float _paceTimer;
    float _realtimePace;
    float _paceDivisor;
    [SerializeField] bool _switchFoot;
    // Start is called before the first frame update
    void Start()
    {
        _pivot = transform.position;
        _paceDivisor = 1 / _pace;
        FootPair.SetPaceDivisor(_paceDivisor);
        FootPair.SetHeight(_height);
    }

    // Update is called once per frame
    void Update()
    {
        _realtimePace = PaceMaker();
        _switchFoot = FootSwitcher(_realtimePace);
        foreach (FootPair footPair in _footPairs)
        {
            footPair.Walk(_realtimePace, _switchFoot);
        }
    }

    bool FootSwitcher(float sqrPace)
    {
        if (SqrHorizontalDist(transform.position, _pivot) > sqrPace)
        {
            _pivot = transform.position;
            return true;
        }
        else
        {
            return false;
        }
    }

    float SqrHorizontalDist(Vector3 a, Vector3 b)
    {
        return Vector3.Scale(Vector3.up + Vector3.right, a - b).sqrMagnitude;
    }

    float PaceMaker()
    {
        return _agent.velocity.magnitude * _pace;
    }
}
