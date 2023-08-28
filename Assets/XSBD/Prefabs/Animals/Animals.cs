using UnityEngine;
using UnityEngine.AI;

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

    [SerializeField] float _eatRange = 1;
    [SerializeField] float _satiety; // ������. ó�� �������� �ִ밪�̶�� ����
    [SerializeField] float _eatAmount;

    [SerializeField] float _animalHP;
    
    [SerializeField] float _walkingSpeed; // ���� ���� �ӵ� �����
    [SerializeField] float _RunningSpeed;
    [SerializeField][Tooltip("small: 0 ~ big: 3, cow is 2")] byte _levelOfSize;

    //Arrays
    [SerializeField] Drop[] _dropsWhenDead;
    [SerializeField] byte[] _foodTypeIndex;
    [SerializeField] Gimmick[] _gimmicks;
    
    STATE _finalState;
    STATE _cognitiveState = STATE.NORMAL; //T
    Vector3 _cognitiveTarget; //Serialized for test
    Transform[] _foodParents;
    Transform[] _foodTargetNominees;
    Transform _foodTaget; //
    Food _food;
    float _originalHP;
    float _originalSatiety; //T
    byte _hungerState; //T
    bool _isEating;
    bool _eatSwitch;

    // Start is called before the first frame update
    void Start()
    {
        if(_agent == null)
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        SetFoodParents();
        _foodTargetNominees = new Transform[_foodTypeIndex.Length];
        _walkingSpeed = _agent.speed;
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
                    if (!_foodTaget)
                    {
                        SetFoodTarget1();
                        SetFoodTarget2();
                    }
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
                    HungerManager();
                    ManageEat();
                    SetFood();
                    break;
                }
            case 4:
                {
                    if (!_foodTaget)
                    {
                        
                    }
                    break;
                }
        }
        MasterManager();
        SetManagedValues();
        Heal();
        Eat();
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
    }

    //Heavy
    void HungerManager()
    {
        if(_foodTaget && (transform.position - _foodTaget.position).sqrMagnitude > _cognitiveDistance * _cognitiveDistance)
        {
            _foodTaget = null;
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

    //�������� ��: ����, ������, ���� Ÿ�� 

    //DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO
    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
    //=====================================================================================================================================

    //Start Functions
    void SetFoodParents()
    {
        _foodParents = FoodManager.GetFoodParents(_foodTypeIndex);
    }

    //Called once in a while
    void ManageGimmicks()
    {
        foreach (Gimmick gimmick in _gimmicks)
        {
            gimmick.ManageBehaviour(_finalState);
        }
    }

    void SetFoodTarget1()
    {
        byte index = 0;
        foreach (Transform foodParent in _foodParents)
        {
            _foodTargetNominees[index] = (FoodManager.SearchUnder(foodParent, transform.position, _cognitiveDistance, transform));
            ++index;
        }
    }

    void SetFoodTarget2()
    {
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

    void SetFood()
    {
        if (_foodTaget)
        {
            _foodTaget.TryGetComponent<Food>(out _food);
        }
    }

    //Called By Managers
    //Stamina Manager
    void DropItems(Drop[] drops)
    {
        foreach (Drop item in drops)
        {
            item.InstantiateDrop(transform.position);
        }
    }

    //Called Every Frame
    void SetManagedValues()
    {
        _satiety -= Time.deltaTime;
        if (_satiety < 0)
        {
            _satiety = 0;
        }
        if (_satiety == 0)
        {
            _animalHP -= Time.deltaTime * 0.0167f;
        }
    }

    void ManageEat()
    {
        if(_hungerState < 2)
        {
            _eatSwitch = true;
        }
        else if(_satiety > _originalSatiety)
        {
            _eatSwitch = false;
        }

        if (_finalState == STATE.NORMAL && _eatSwitch && _foodTaget
            && Vector3.SqrMagnitude(transform.position - _foodTaget.position) < _eatRange * _eatRange)
        {
            _isEating = true;
        }
        else
        {
            _isEating = false;
        }
    }

    void Eat()
    {
        if (_isEating)
        {
            _food.DecreaseHP(_eatAmount * Time.deltaTime);
            _satiety += _food.GetEfficiency() * _eatAmount * Time.deltaTime;
        }
    }

    void Heal()
    {
        if(_animalHP < _originalHP && _satiety > 0)
        {
            _satiety -= Time.deltaTime;
            _animalHP += Time.deltaTime * 0.03125f;
        }
    }

    //=====================================================================================================================================
    float StateMoveSpeedManager(STATE finalState) // ���¿� ���� �̵��ӵ� ����
    {
        // chase run normal hungry
      
        if(finalState == STATE.CHASE || finalState == STATE.RUN)
        {
            _agent.speed = _walkingSpeed;
        }
        else if (finalState == STATE.NORMAL)
        {
            _agent.speed = _RunningSpeed;
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

    public void DecreaseHP(float amount)
    {
        _animalHP -= amount;
    }

    public float GetSatiety()
    {
        return _satiety;
    }

    public bool GetFullness()
    {
        return _satiety - _originalSatiety > 0;
    }

    public byte GetSize()
    {
        return _levelOfSize;
    }

    public byte GetHungerState()
    {
        return _hungerState;
    }

    public Vector3 GetFoodTarget(Vector3 position)
    {
        if (_foodTaget)
        {
            return _foodTaget.position;
        }
        else
        {
            return position;
        }
    }

    public bool GetIsEating()
    {
        return _isEating;
    }

    //DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO
    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
}
