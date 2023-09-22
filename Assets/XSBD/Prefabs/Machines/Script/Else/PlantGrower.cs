using UnityEngine;
using UnityEngine.Rendering;

public class PlantGrower : MonoBehaviour
{
    [SerializeField] GameObject[] _growthPrefabs;
    [SerializeField] float _averageLengthPerStage = 30;
    [SerializeField] float _stageRandomness = 10;
    Transform _plant;
    [SerializeField] float _timer;
    [SerializeField] bool _randomizeYRotation = true;

    byte _index = 0;

    // Update is called once per frame
    void Update()
    {
        Grow();
    }

    void Grow()
    {
        SetTimer();
        if(_timer == 0)
        {
            if (_index < _growthPrefabs.Length)
            {
                if (_plant)
                {
                    Destroy(_plant.gameObject);
                }
                InstantiatePlant();
                if(_index < _growthPrefabs.Length)
                {
                    _timer = SetStageLength();
                }
            }
            else
            {
                ResetIndex();
            }
        }
    }

    void SetTimer()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        if (_timer < 0)
        {
            _timer = 0;
        }
    }

    void InstantiatePlant()
    {
        if (_randomizeYRotation)
        {
            float _rotY = 360 * Random.value;
            Debug.Log(_rotY);
            transform.rotation *= Quaternion.Euler(Vector3.up * _rotY);
        }
        _plant = Instantiate(_growthPrefabs[_index], transform).transform;

        if (_growthPrefabs[_index].TryGetComponent<Item>(out Item item))
        {
            item = _plant.GetComponent<Item>();
            _plant.parent = item.GetParent();
        }
        _index++;
    }

    void ResetIndex()
    {
        if(_index >= _growthPrefabs.Length)
        {
            if (!_plant)
            {
                _index = 0;
                
            }
            else if(Vector3.SqrMagnitude(_plant.position - transform.position) > 1)
            {
                _index = 0;
                if(_plant.parent == transform)
                {
                    _plant.parent = null;
                }
                _plant = null;
            }
        }
    }

    float SetStageLength()
    {
        return _averageLengthPerStage + (Random.value - 0.5f) * _stageRandomness;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == _plant.gameObject)
        {
            _index = 0;
        }
    }
}
