using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("NavMesh and Speed")]
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Gimmick[] _gimmicks;
    [SerializeField] Transform _mouth;
    [SerializeField] float _cognitiveDistance;
    [SerializeField] [Tooltip("small: 0 ~ big: 3, cow is 2")] byte _levelOfSize;

    //Serialized for test purposes
    [SerializeField] STATE _finalState;
    [SerializeField] Vector3 _finalTarget;

    //HpManager Values
    float _originalHP;
    [SerializeField] float _animalHP;

    //HungerManager Values
    [SerializeField] Transform[] _foodParents;
    [SerializeField] Transform[] _foodTargetNominees;
    [SerializeField] Transform _hungerTaget;
    [SerializeField] float _originalSatiety;
    [SerializeField] float _satiety; // ������. ó�� �������� �ִ밪�̶�� ����
    [SerializeField] byte _hungerState;

    float _originalSpeed; // ���� ���� �ӵ� �����

    //CognitiveManager values
    STATE _cognitiveState = STATE.NORMAL;
    [SerializeField] Vector3 _cognitiveTarget; //Serialized for test

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _foodTargetNominees = new Transform[_foodParents.Length];
        _originalSpeed = _agent.speed;
        _originalSatiety = _satiety;
        _originalHP = _animalHP;
    }

    // Update is called once per frame
    void Update()
    {
        ManageMoveSpeed();
        ManageGimmicks();
        switch (AnimalManager._frameCounter)
        {
            case 0:
                {
                    CognitiveManager(_levelOfSize);
                    break;
                }
            case 1:
                {
                    HungerManager();
                    break;
                }
            case 2:
                {
                    break;
                }
            case 3:
                {
                    break;
                }
            case 4:
                {
                    break;
                }
        }
        _agent.SetDestination(_finalTarget);
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
        _cognitiveTarget = AnimalManager.CrudeFlee(transform.position, _cognitiveDistance, animalSize) * _agent.speed * 10;
        if (_cognitiveTarget == Vector3.zero)
        {
            _cognitiveState = STATE.NORMAL;
        }
        else
        {
            _cognitiveState = STATE.RUN;
        }
        //Debug.Log(AnimalManager.Search(transform.position, _cognitiveDistance, animalSize).Count);
    }

    void HungerManager()
    {
        void SetTarget()
        {
            float resSqrDist = 0;
            foreach (Transform foodParent in _foodParents)
            {
                Transform nominee = FoodManager.SearchUnder(foodParent, transform.position, _cognitiveDistance, transform);
                float sqrDistance = (nominee.position - transform.position).sqrMagnitude;
                if (sqrDistance > 0 && (sqrDistance < resSqrDist || resSqrDist == 0))
                {
                    resSqrDist = sqrDistance;
                    _hungerTaget = nominee;
                }
            }
        }
        if (_satiety < _originalSatiety * 0.125f)
        {
            _hungerState = 0;
            if(_hungerTaget is null)
            {
                SetTarget();
            }
        }
        else if(_satiety < _originalSatiety * 0.125f * 7)
        {
            _hungerState = 1;
            if (_hungerTaget is null)
            {
                SetTarget();
            }
        }
        else
        {
            _hungerState = 2;
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

    //Public Methods=======================================================================================================================
    public void DecreaseSatiety(float amount)
    {
        _satiety -= amount;
    }

    public float GetSatiety()
    {
        return _satiety;
    }
}
