using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] GameObject _item;
    [SerializeField] Transform _parent;
    [SerializeField] byte _quantity;
    [SerializeField] bool _stackable;

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
