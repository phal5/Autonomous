using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stroll : MonoBehaviour
{
    [SerializeField] float _avgMoveTime = 3f;
    [SerializeField] float _avgRestTime = 5f;
    [SerializeField] float _strollSpeed = 1f;
    NavMeshAgent _agent;
    float _steer = 0.5f;
    float _timer;
    bool _move = true;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (!_agent.isOnNavMesh) Debug.Log(gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    void Move()
    {
        _agent.speed = _strollSpeed;
        _steer += Time.deltaTime * (Random.value * 2 - 1);
        _steer = Mathf.Clamp(_steer, -1, 1) * 0.99f;
        _agent.SetDestination(transform.forward + transform.position + transform.right * _steer);
    }

    void Stop()
    {
        _agent.speed = 0;
        _agent.SetDestination(transform.forward);
    }

    void Timer()
    {
        if (_move)
        {
            if(_timer > 0)
            {
                Move();
            }
            else
            {
                _timer = SetRandomized(_avgRestTime, 0.5f);
                _steer = 0;
                _move = false;
            }
        }
        else
        {
            if (_timer > 0)
            {
                Stop();
            }
            else
            {
                _timer = SetRandomized(_avgMoveTime, 0.5f);
                _move = true;
            }
        }
        _timer -= Time.deltaTime;
    }

    float SetRandomized(float value, float randomness)
    {
        return value + (Random.value * 2 - 1) * randomness * value;
    }
}
