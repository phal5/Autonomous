using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animals : MonoBehaviour
{
    [System.Serializable]
    class Gimmick
    {
        [SerializeField] Behaviour _behaviour;
        enum State {a, b, c};
        [SerializeField] State _state;
        
        public void ManageInability(int state)
        {
            if((int)_state == state)
            {
                Debug.Log("Yep");
                _behaviour.enabled = true;
            }
            else
            {
                Debug.Log("Nup");
                _behaviour.enabled = false;
            }
        }
    }

    [SerializeField] int _finalState;
    [SerializeField] Vector3 _finalTarget;
    [SerializeField] Gimmick[] _gimmicks;
    [SerializeField] NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_finalTarget);
    }

    void ManageGimmicks()
    {
        foreach (Gimmick gimmick in _gimmicks)
        {
            gimmick.ManageInability(_finalState);
        }
    }
}
