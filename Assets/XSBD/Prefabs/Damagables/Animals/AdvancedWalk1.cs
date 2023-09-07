using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
        static float _paceDivisor;
        static float _height;
        static bool _12;
        
        public static void SetPaceDivisor(float paceDivisor)
        {
            _paceDivisor = paceDivisor;
        }

        public static void SetBaseHeight(float height)
        {
            _height = height;
        }

        private float SetHeight(float sqrPace, float sqrDist)
        {
            return (sqrPace - sqrDist) * _paceDivisor * _paceDivisor;
        }

        private bool Cast(Transform hip, Vector3 offset, out RaycastHit hit)
        {
            return Physics.Raycast(hip.position + offset, Vector3.down, out hit, 2);
        }

        private void Step(Transform foot, Transform hip, Vector3 fixedFoot, Vector3 fixedHip)
        {

        }
    }

    [SerializeField] NavMeshAgent _agent;
    [SerializeField] FootPair[] _footPairs;

    [Tooltip("Pace when Speed = 1")]
    [SerializeField] float _standardPace;
    [SerializeField] float _minPaceTime = 0.2f;
    [SerializeField] float _height;

    float _pace;
    [SerializeField] bool _switchFoot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPace()
    {
        _pace = _agent.velocity.sqrMagnitude * _pace;
    }
}
