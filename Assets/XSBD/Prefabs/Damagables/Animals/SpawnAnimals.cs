using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimals : MonoBehaviour
{
    [Header("Spawns in the verge of circle with radius of half of range")]
    [SerializeField] int _maxInRange;
    [SerializeField] float _range;

    List<Transform> _spawnAnimals = new List<Transform>();
    Transform _playerTransform;
    RaycastHit _hit;
    Vector3 _circular;
    float _sqrRange;

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = PlayerManager.GetPlayer().transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (AnimalManager._frameCounter)
        {
            case 2:
                InstantiateAnimal((byte)(int)(Random.value * AnimalManager._AnimalPrefabs.Length));
                break;
            case 3:
                DestroyIfFarOrNull();
                break;
        }
    }

    private void InstantiateAnimal(byte index)
    {
        if (AnimalManager.HowManyAround(PlayerManager.GetPlayerPosition(), _range, 1, false) < _maxInRange)
        {
            _circular = Random.insideUnitCircle.x * Vector3.right + Random.insideUnitCircle.y * Vector3.forward;
            _circular = _circular.normalized * _range * 0.5f;
            _circular += PlayerManager.GetPlayerPosition();

            if (Physics.Raycast(_circular, Vector3.up, out _hit)) Physics.Raycast(_hit.point, Vector3.down, out _hit);
            else Physics.Raycast(_circular + Vector3.up * 100, Vector3.down, out _hit);

            if(!_hit.collider.isTrigger) _spawnAnimals.Add(AnimalManager.InstantiateAnimal(index, _hit.point + Vector3.up).transform);
        }
    }

    private void DestroyIfFarOrNull()
    {
        foreach (Transform t in _spawnAnimals)
        {
            if (t)
            {
                if ((_playerTransform.position - t.position).sqrMagnitude > _range * _range)
                {
                    _spawnAnimals.Remove(t);
                    Destroy(t.gameObject);
                    Debug.Log("yep");
                }
            }
            else _spawnAnimals.Remove(t);
        }
    }
}
