using System;
using UnityEngine;
using UnityEngine.AI;

public class Animals : Damagable
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

    [Space(10f)]
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform _mouth;
    [SerializeField] float _cognitiveDistance;

    [Space(10f)]
    [SerializeField] float _eatRange = 1;
    [SerializeField] float _MaxSatiety; // 포만감. 처음 포만감은 최대값이라고 가정
    [SerializeField] float _satiety;
    [SerializeField] float _eatAmount;

    [Space(10f)]
    [SerializeField] float _walkingSpeed; // 기존 동물 속도 저장용
    [SerializeField] float _RunningSpeed;
    [SerializeField][Tooltip("small: 0 ~ big: 3, cow is 2")] byte _levelOfSize;

    //Arrays
    [Space(10f)]
    [SerializeField] byte[] _foodTypeIndex;
    [SerializeField] Gimmick[] _gimmicks;

    [Space(10f)]
    [SerializeField] bool _isPredator = false;
    
    STATE _finalState;
    STATE _cognitiveState = STATE.NORMAL;
    Vector3 _cognitiveTarget;
    RaycastHit _hit;
    Transform[] _foodParents;
    Transform[] _foodTargetNominees;
    Transform _foodTaget;
    Food _food;
    float _foodTimer = 0;
    float _runTimer = 0;
    byte _hungerState;
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
        _HP = _MaxHP;
        _foodTargetNominees = new Transform[_foodTypeIndex.Length];
        _walkingSpeed = _agent.speed;
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
                        _foodTimer = 0;
                    }
                    break;
                }
            case 2:
                {
                    ManageGimmicks();
                    ManageMoveSpeed();
                    break;
                }
            case 3:
                {
                    HungerManager();
                    ManageEat();
                    HPManager();
                    SetFood();
                    break;
                }
            case 4:
                {
                    if (_foodTimer > 10)
                    {
                        _foodTaget = null;
                    }
                    break;
                }
        }
        MasterManager();
        SetManagedValues();
        Heal();
        Eat();
        _foodTimer += Time.deltaTime;
    }

    //=====================================================================================================================================
    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
    //MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGERS MANAGER

    //Heavy
    void CognitiveManager(byte animalSize)
    {
        ++animalSize;

        _cognitiveTarget = AnimalManager.CrudeFlee(transform.position, _cognitiveDistance, animalSize);
        if (_runTimer > 0) _cognitiveTarget += (_cognitiveDistance * _cognitiveDistance - (transform.position - PlayerManager.GetPlayerPosition()).sqrMagnitude) * (transform.position - PlayerManager.GetPlayerPosition()).normalized;
        
        if (_cognitiveTarget == Vector3.zero)
        {
            //Search for Chase target

            //If there isn't any, set state to normal
            _cognitiveState = STATE.NORMAL;
            _cognitiveTarget = transform.position;
        }
        else
        {
            _cognitiveTarget = _cognitiveTarget.normalized * 10 * _agent.speed;
            if (Physics.Raycast(transform.position, _cognitiveTarget, out _hit, 10 * _agent.speed)) _cognitiveTarget = _hit.point;
            else _cognitiveTarget += transform.position;
            _cognitiveState = STATE.RUN;
        }
    }

    //Heavy
    void HungerManager()
    {
        if(_foodTaget && (transform.position - _foodTaget.position).sqrMagnitude > _cognitiveDistance * _cognitiveDistance)
        {
            _foodTaget = null;
        }
        if (_satiety < _MaxSatiety * 0.125f)
        {
            _hungerState = 0;
        }
        else if(_satiety < _MaxSatiety * 0.125f * 7)
        {
            _hungerState = 1;
        }
        else
        {
            _hungerState = 2;
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
                _finalState = STATE.CHASE;
            }
        }
        else
        {
            _finalState = _cognitiveState;
            _agent.SetDestination(_cognitiveTarget);
        }
    }

    //정해지는 것: 상태, 목적지, 음식 타겟 

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
    

    //Called Every Frame
    void SetManagedValues()
    {
        //less efficient
        //if(((_satiety > 0)? (_satiety -= Time.deltaTime) : (_satiety = 0)) == 0)
        //{
        //    _HP -= Time.deltaTime * 0.0167f;
        //}

        if (_satiety > 0) _satiety -= Time.deltaTime;
        else
        {
            _satiety = 0;
            _HP -= Time.deltaTime * 0.0167f;
        }

        if (_runTimer > 0) _runTimer -= Time.deltaTime;
        else _runTimer = 0;

    }

    void ManageEat()
    {
        if(_hungerState < 2)
        {
            _eatSwitch = true;
        }
        else if(_satiety > _MaxSatiety)
        {
            _eatSwitch = false;
        }

        if (_finalState == STATE.CHASE && _eatSwitch && _foodTaget
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
        if(_HP < _MaxHP && _satiety > 0)
        {
            _satiety -= Time.deltaTime;
            _HP += Time.deltaTime * 0.03125f;
        }
    }

    //=====================================================================================================================================
    float StateMoveSpeedManager(STATE finalState) // 상태에 따른 이동속도 조절
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

        // 아직 테스트 X
        float hungerRatio = 1 + 0.3f * (1 - _satiety / _MaxSatiety); // 0~1까지의 비율 
        _agent.speed *= hungerRatio; // 포만감 고려. 일단 최대로 배고플 시 기존 속도의 1.3만큼 곱해지기로 만듬

        return _agent.speed;
    }

    void ManageMoveSpeed() // Update에 들어가는 이동 메소드. 
    {

        // 평상시 걷기, 달리기 | 지쳤을 때 걷기, 달리기 속도 다름.
        float hp_Threshold = _MaxHP * 0.3f; // 임계점은 동물 체력의 30%

        if (_HP <= hp_Threshold) // 임계점 이상의 체력일 때 이동속도는 빠르다.
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

    public bool GetFullness()
    {
        return _satiety - _MaxSatiety > 0;
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

    public override void DecreaseHP(bool didPlayerHit, float amount = 1)
    {
        base.DecreaseHP(didPlayerHit, amount);
        if (didPlayerHit) _runTimer = 10;
    }

    //DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO
    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
}
