using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [System.Serializable] class Drop
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] Transform _parent;
        [SerializeField] float _probability;

        public void InstantiateDrop(Vector3 position)
        {
            if (Random.value < _probability)
            {
                Instantiate(_prefab, position, Random.rotation, _parent);
            }
        }
    }

    [Header("Damagable")]
    [SerializeField] protected float _MaxHP;
    [SerializeField] protected float _HP;
    [SerializeField] Drop[] _dropsWhenDead;

    // Start is called before the first frame update
    void Start()
    {
        _HP = _MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        HPManager();
    }

    public void DecreaseHP(float amount = 1)
    {
        _HP -= amount;
    }

    void DropItems(Drop[] drops)
    {
        foreach (Drop item in drops)
        {
            item.InstantiateDrop(transform.position);
        }
    }

    protected void HPManager()
    {
        if (_HP <= 0)
        {
            DropItems(_dropsWhenDead);
            Destroy(gameObject);
        }
    }
}
