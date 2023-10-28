using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [System.Serializable] class Drop
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] ParentData _parentData;
        [SerializeField] float _probability;
        [Space(10f)]
        [SerializeField] Vector3 _localDropOffset = Vector3.zero;

        public void InstantiateDrop(Vector3 position)
        {
            if (Random.value < _probability)
            {
                Instantiate(_prefab, position, Random.rotation, _parentData.GetParent()).transform.localPosition += _localDropOffset;
            }
        }
    }

    [Header("Damagable")]
    [SerializeField] protected float _MaxHP;
    [SerializeField] Drop[] _dropsWhenDead;
    protected float _HP;

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
