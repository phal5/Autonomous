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
    class Drop
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

    [Header("NavMesh and Speed")]
    [SerializeField] NavMeshAgent _agent;
    float _originalSpeed; // 기존 동물 속도 저장용

    [SerializeField] Gimmick[] _gimmicks;

    [Header("Perception Variables")]
    [SerializeField] Transform _mouth;
    [SerializeField] float _cognitiveDistance;
    [SerializeField] [Tooltip("small: 0 ~ big: 3, cow is 2")] byte _levelOfSize;

    //Serialized for test purposes
    [SerializeField] STATE _finalState;
    [SerializeField] Vector3 _finalTarget;

    //HpManager Values
    float _originalHP;
    [SerializeField] float _animalHP;
    [SerializeField] Drop[] _dropsWhenDead;

    //HungerManager Values
    [SerializeField] Transform[] _foodParents;
    [SerializeField] Transform[] _foodTargetNominees; //T
    [SerializeField] Transform _foodTaget; //T
    [SerializeField] float _originalSatiety; //T
    [SerializeField] float _satiety; // 포만감. 처음 포만감은 최대값이라고 가정
    [SerializeField] byte _hungerState; //T

    //CognitiveManager values
    STATE _cognitiveState = STATE.NORMAL; //T
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
        StaminaManager();
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
        MasterManager();
        Move();
    }

    //Heavy
    void CognitiveManager(byte animalSize)
    {
        ++animalSize;
        
        _cognitiveTarget = AnimalManager.CrudeFlee(transform.position, _cognitiveDistance, animalSize) * _agent.speed * 10;
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
                    _foodTaget = nominee;
                }
            }
        }
        if (_satiety < _originalSatiety * 0.125f)
        {
            _hungerState = 0;
            if(_foodTaget is null)
            {
                SetTarget();
            }
        }
        else if(_satiety < _originalSatiety * 0.125f * 7)
        {
            _hungerState = 1;
            if (_foodTaget is null)
            {
                SetTarget();
            }
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
        switch (_cognitiveState)
        {
            case STATE.NORMAL:
                {
                    if(_hungerState < 2)
                    {
                        _finalTarget = _foodTaget.position;
                    }
                    else
                    {
                        _finalTarget = _cognitiveTarget;
                    }
                    _finalState = _cognitiveState;
                    break;
                }
            case STATE.CHASE:
                {
                    if (_hungerState == 0)
                    {
                        _finalTarget = _foodTaget.position;
                        _finalState = STATE.NORMAL;
                    }
                    else
                    {
                        _finalTarget = _cognitiveTarget;
                        _finalState = _cognitiveState;
                    }
                    break;
                }
            case STATE.RUN:
                {
                    _finalTarget = _cognitiveTarget;
                    _finalState = _cognitiveState;
                    break;
                }
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

    //light
    float StateMoveSpeedManager(STATE finalState) // 상태에 따른 이동속도 조절
    {
        // chase run normal hungry
        float runRatio = 1.3f; // 달리기 시 속도 비율 
      
        if(finalState == STATE.CHASE || finalState == STATE.RUN)
        {
            _agent.speed = _originalSpeed * runRatio;
        }
        else if (finalState == STATE.NORMAL)
        {
            _agent.speed = _originalSpeed;
        }

        // 아직 테스트 X
        float hungerRatio = 1 + 0.3f * (1 - _satiety / _originalSatiety); // 0~1까지의 비율 
        _agent.speed *= hungerRatio; // 포만감 고려. 일단 최대로 배고플 시 기존 속도의 1.3만큼 곱해지기로 만듬

        return _agent.speed;
    }

    void ManageMoveSpeed() // Update에 들어가는 이동 메소드. 
    {

        // 평상시 걷기, 달리기 | 지쳤을 때 걷기, 달리기 속도 다름.
        float hp_Threshold = _originalHP * 0.3f; // 임계점은 동물 체력의 30%

        if (_animalHP <= hp_Threshold) // 임계점 이상의 체력일 때 이동속도는 빠르다.
            _agent.speed = StateMoveSpeedManager(_finalState) * 0.3f;
        else 
            StateMoveSpeedManager(_finalState);
    }

    void DropItems(Drop[] drops)
    {
        foreach(Drop item in drops)
        {
            item.InstantiateDrop(transform.position);
        }
    }

    void Move()
    {
        _agent.SetDestination(_finalTarget);
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

    public float GetSpeed()
    {
        return _agent.speed;
    }
}
