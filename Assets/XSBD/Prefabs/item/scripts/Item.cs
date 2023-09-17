using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    enum Parent { ANIMAL, FOOD, NONE }
    [SerializeField] GameObject _item;
    [SerializeField] byte _quantity;
    [SerializeField] bool _stackable;
    [Space(10f)]
    [SerializeField] Transform _parent;
    [SerializeField] Parent _type;
    [SerializeField] byte _parentIndex;

    private void Start()
    {
        switch (_type)
        {
            case Parent.ANIMAL:
                _parent = AnimalManager.GetAnimalParent(_parentIndex);
                break;

            case Parent.FOOD:
                _parent = FoodManager.GetFoodParent(_parentIndex);
                break;

            default:
                _parent = null;
                break;
        }
    }

    public void GetItemData(out GameObject item, out Transform parent, out byte quantity, out bool stackable)
    {
        item = _item;
        parent = _parent;
        quantity = _quantity;
        stackable = _stackable;
        Destroy(gameObject);
    }

    public GameObject GetItem()
    {
        return _item;
    }
    public Transform GetParent()
    {
        return _parent;
    }
    public byte GetQuantity()
    {
        return _quantity;
    }
    public bool GetStackable()
    {
        return _stackable;
    }

    public void DisableStackable()
    {
        _stackable = false;
        _item = gameObject;
    }
}
