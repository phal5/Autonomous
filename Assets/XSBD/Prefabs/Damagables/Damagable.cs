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
        [SerializeField] float _probability = 1;
        [Space(10f)]
        [SerializeField] Vector3 _localDropOffset = Vector3.zero;
        [SerializeField] bool _randomizeRotation = true;

        public void InstantiateDrop(Transform transform)
        {
            if (Random.value < _probability)
            {
                Instantiate(_prefab, transform.position, (_randomizeRotation? Random.rotation : transform.rotation), _parentData.GetParent()).transform.localPosition += _localDropOffset;
            }
        }
    }

    [Header("Damagable")]
    [SerializeField] protected float _MaxHP;
    [SerializeField] Drop[] _dropsWhenDead;
    [SerializeField] protected float _HP;

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

    public virtual void DecreaseHP(bool didPlayerHit, float amount = 1)
    {
        _HP -= amount;
    }

    void DropItems(Drop[] drops)
    {
        foreach (Drop item in drops)
        {
            item.InstantiateDrop(transform);
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
