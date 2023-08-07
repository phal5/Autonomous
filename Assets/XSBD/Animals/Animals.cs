using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Animals : MonoBehaviour
{
    enum STATE { NORMAL, CHASE, RUN};

    [System.Serializable]
    class Gimmick
    {
        [SerializeField] Behaviour _behaviour;
        [SerializeField] STATE _state;

        public void ManageBehaviour(STATE state)
        {
            if(_state == state)
            {
                _behaviour.enabled = true;
            }
            else
            {
                _behaviour.enabled = false;
            }
        }
    }

    [SerializeField] Gimmick[] _gimmicks;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] float _cognitiveDistance;
    [SerializeField][Tooltip("small: 0 ~ big: 3, cow is 2")] byte _levelOfSize;

    //Serialized for test purposes
    [SerializeField] STATE _finalState;
    [SerializeField] Vector3 _finalTarget;

    float _originalHP;
    [SerializeField] float _animalHP;

    float _originalSatiety;
    [SerializeField] float _satiety; // ������. ó�� �������� �ִ밪�̶�� ����.

    float _originalSpeed; // ���� ���� �ӵ� �����

    //CognitiveManager values
    STATE _cognitiveState = STATE.NORMAL;
    [SerializeField] Vector3 _cognitiveTarget; //Serialized for test

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
        if(AnimalManager._frameCounter == 0)//
        {
            CognitiveManager(_levelOfSize);
        }
    }

    

    void ManageGimmicks()
    {
        foreach (Gimmick gimmick in _gimmicks)
        {
            gimmick.ManageBehaviour(_finalState);
        }
    }

    float StateMoveSpeedManager(STATE finalState) // ���¿� ���� �̵��ӵ� ����
    {
        // chase run normal hungry
        float runRatio = 1.3f; // �޸��� �� �ӵ� ���� 
      
        if(finalState == STATE.CHASE || finalState == STATE.RUN)
        {
            _agent.speed = _originalSpeed * runRatio;
        }
        else if (finalState == STATE.NORMAL)
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

    void CognitiveManager(byte animalSize)
    {
        ++animalSize;
        /*
        List<Vector3> SensePredator()
        {
            List<Vector3> result = new List<Vector3>();
            List<Vector3> predators = AnimalManager.Search(transform.position, _cognitiveDistance, ++animalSize);
            
            return result;
        }
        */
        _cognitiveTarget = AnimalManager.CrudeFlee(transform.position, _cognitiveDistance, animalSize);
        if(_cognitiveTarget == Vector3.zero)
        {
            _cognitiveState = STATE.NORMAL;
        }
        else
        {
            _cognitiveState = STATE.RUN;
        }
        //Debug.Log(AnimalManager.Search(transform.position, _cognitiveDistance, animalSize).Count);
    }

    public void DecreaseSatiety(float amount)
    {
        _satiety -= amount;
    }
}
