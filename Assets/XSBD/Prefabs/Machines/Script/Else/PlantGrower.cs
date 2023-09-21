using UnityEngine;

public class PlantGrower : MonoBehaviour
{
    [SerializeField] GameObject[] _growthPrefabs;
    [SerializeField] float _averageLengthPerStage = 30;
    [SerializeField] float _stageRandomness = 10;
    Transform _plant;
    [SerializeField] float _timer;
    byte _index = 0;

    // Update is called once per frame
    void Update()
    {
        Grow();
    }

    void Grow()
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        if( _timer < 0)
        {
            _timer = 0;
        }
        if(_timer == 0)
        {
            if (_index < _growthPrefabs.Length)
            {
                if (_plant)
                {
                    Destroy(_plant.gameObject);
                }
                _plant = Instantiate(_growthPrefabs[_index++], transform).transform;
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

    void ResetIndex()
    {
        if(_index >= _growthPrefabs.Length && Vector3.SqrMagnitude(_plant.localPosition) > 1)
        {
            _index = 0;
            _plant.parent = null;
            _plant = null;
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
