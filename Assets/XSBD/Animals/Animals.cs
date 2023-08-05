using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Animals : MonoBehaviour
{
    enum STATE { NORMAL, CHASE, RUN, HUNGRY };

    [System.Serializable]
    class Gimmick
    {
        [SerializeField] Behaviour _behaviour;
        [SerializeField] STATE _state;

        public void ManageBehaviour(int state)
        {
            if((int)_state == state)
            {
                _behaviour.enabled = true;
            }
            else
            {
                _behaviour.enabled = false;
            }
        }
    }

    [SerializeField] int _finalState;
    [SerializeField] Vector3 _finalTarget;
    [SerializeField] Gimmick[] _gimmicks;
    [SerializeField] NavMeshAgent _agent;

    [SerializeField] float _animalHP;
    [SerializeField] float _satiety; // ������. ó�� �������� �ִ밪�̶�� ����.

    float _originalSpeed; // ���� ���� �ӵ� �����
    float _originalSatiety;
    float _originalHP;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _originalSpeed = _agent.speed;
        _originalSatiety = _satiety;
        _originalHP = _animalHP;
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_finalTarget);

        ManageMoveSpeed();
        ManageGimmicks();
    }

    

    void ManageGimmicks()
    {
        foreach (Gimmick gimmick in _gimmicks)
        {
            gimmick.ManageBehaviour(_finalState);
        }
    }

    float StateMoveSpeedManager(int finalState) // ���¿� ���� �̵��ӵ� ����
    {
        // chase run normal hungry
        float runRatio = 1.3f; // �޸��� �� �ӵ� ���� 
      
        if(finalState == (int)STATE.CHASE || finalState == (int)STATE.RUN)
        {
            _agent.speed = _originalSpeed * runRatio;
        }
        else if (finalState == (int)STATE.NORMAL)
        {
            _agent.speed = _originalSpeed;
        }

        // ���� �׽�Ʈ X
        float hungerRatio = 1 + 0.3f * (1 - _satiety / _originalSatiety); // 0~1������ ���� 
        _agent.speed *= hungerRatio; // ������ ���. �ϴ� �ִ�� ����� �� ���� �ӵ��� 1.3��ŭ ��������� ����

        return _agent.speed;
    }

    void ManageMoveSpeed() // Update�� ���� �̵� �޼ҵ�. 
    {

        // ���� �ȱ�, �޸��� | ������ �� �ȱ�, �޸��� �ӵ� �ٸ�.
        float hp_Threshold = _originalHP * 0.3f; // �Ӱ����� ���� ü���� 30%

        if (_animalHP <= hp_Threshold) // �Ӱ��� �̻��� ü���� �� �̵��ӵ��� ������.
            _agent.speed = StateMoveSpeedManager(_finalState) * 0.3f;
        else 
            StateMoveSpeedManager(_finalState);
    }
}
