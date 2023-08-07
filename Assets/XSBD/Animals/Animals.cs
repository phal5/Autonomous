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
    [SerializeField] float _satiety; // 포만감. 처음 포만감은 최대값이라고 가정.

    float _originalSpeed; // 기존 동물 속도 저장용

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
