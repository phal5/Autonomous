using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Animals : MonoBehaviour
{
    enum STATE { NORMAL, CHASE, RUN};
    [System.Serializable] class Gimmick
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
    [System.Serializable] class Drop
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] Transform _parent;
        [SerializeField] float _probability;

        public void InstantiateDrop(Vector3 position)
        {
            if(Random.value < _probability)
            {
                Instantiate(_prefab, position, Random.rotation, _parent);
            }
        }
    }

    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform _mouth;
    [SerializeField] float _cognitiveDistance;
    [SerializeField] float _satiety; // ������. ó�� �������� �ִ밪�̶�� ����
    [SerializeField] float _animalHP;
    [SerializeField][Tooltip("small: 0 ~ big: 3, cow is 2")] byte _levelOfSize;

    [SerializeField] Drop[] _dropsWhenDead;
    [SerializeField] Transform[] _foodParents;
    [SerializeField] Gimmick[] _gimmicks;
    
    STATE _finalState;
    STATE _cognitiveState = STATE.NORMAL; //T
    Vector3 _cognitiveTarget; //Serialized for test
    List<Transform> _foodTargetNominees = new List<Transform>();
    Transform _foodTaget; //T
    float _originalSpeed; // ���� ���� �ӵ� �����
    float _originalHP;
    float _originalSatiety; //T
    byte _hungerState; //T

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
                    ManageGimmicks();
                    ManageMoveSpeed();
                    StaminaManager();
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
        MasterManager();
        SetManagedValues();
    }

    //=====================================================================================================================================
    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
    //MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGER

    //Heavy
    void CognitiveManager(byte animalSize)
    {
        ++animalSize;
        
        _cognitiveTarget = AnimalManager.CrudeFlee(transform.position, _cognitiveDistance, animalSize).normalized * _agent.speed * 10;
        if (_cognitiveTarget == Vector3.zero)
        {
            //Search for Chase target

            //If there isn't any, set state to normal
            _cognitiveState = STATE.NORMAL;
            _cognitiveTarget = transform.position;
        }
        else
        {
            _cognitiveState = STATE.RUN;
            _cognitiveTarget += transform.position;
        }
        /*
        List<Vector3> SensePredator()
        {
            List<Vector3> result = new List<Vector3>();
            List<Vector3> predators = AnimalManager.Search(transform.position, _cognitiveDistance, ++animalSize);
            
            return result;
        }
        */
        //Debug.Log(AnimalManager.Search(transform.position, _cognitiveDistance, animalSize).Count);
    }

    //Heavy
    void HungerManager()
    {
        void SetFoodTarget()
        {
            _foodTargetNominees = new List<Transform>();
            foreach (Transform foodParent in _foodParents)
            {
                _foodTargetNominees.Add(FoodManager.SearchUnder(foodParent, transform.position, _cognitiveDistance, transform));
            }
            float resSqrDist = 0;
            foreach (Transform nominee in _foodTargetNominees)
            {
                float sqrDistance = (nominee.position - transform.position).sqrMagnitude;
                if (sqrDistance > 0 && (sqrDistance < resSqrDist || resSqrDist == 0))
                {
                    resSqrDist = sqrDistance;
                    _foodTaget = nominee;
                }
            }
        }

        if (_satiety < _originalSatiety * 0.125f)
        {
            _hungerState = 0;
        }
        else if(_satiety < _originalSatiety * 0.125f * 7)
        {
            _hungerState = 1;
        }
        else
        {
            _hungerState = 2;
        }
        if (!_foodTaget)
        {
            SetFoodTarget();
        }
    }

    //Very light
    void StaminaManager()
    {
        if(_animalHP <= 0)
        {
            DropItems(_dropsWhenDead);
            Destroy(gameObject);
        }
    }

    //light
    void MasterManager()
    {
        if((int)_cognitiveState < 2 - _hungerState)
        {
            _finalState = STATE.NORMAL;
            if(!_foodTaget)
            {
                _agent.SetDestination(transform.position);
            }
            else
            {
                _agent.SetDestination(_foodTaget.position);
            }
        }
        else
        {
            _finalState = _cognitiveState;
            _agent.SetDestination(_cognitiveTarget);
        }
    }

    //DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO
    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
    //=====================================================================================================================================

    void SetManagedValues()
    {
        _satiety -= Time.deltaTime;
        if( _satiety < 0)
        {
            _satiety = 0;
        }
        if (_satiety == 0)
        {
            _animalHP -= Time.deltaTime * 0.0167f;
        }
    }

    //light
    void ManageGimmicks()
    {
        foreach (Gimmick gimmick in _gimmicks)
        {
            gimmick.ManageBehaviour(_finalState);
        }
    }

    void DropItems(Drop[] drops)
    {
        foreach (Drop item in drops)
        {
            item.InstantiateDrop(transform.position);
        }
    }



    //=====================================================================================================================================
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
    //=====================================================================================================================================



    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
    //PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC

    public void DecreaseSatiety(float amount)
    {
        _satiety -= amount;
    }

    public float GetSatiety()
    {
        return _satiety;
    }

    public float GetSpeed()
    {
        return _agent.speed;
    }

    //DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO
    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
}
