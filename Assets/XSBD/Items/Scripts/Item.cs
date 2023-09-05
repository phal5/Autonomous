using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Transform _parent;
    byte _quantity;
    bool _isStackable;

    public void GetItemData(ref GameObject itemtype, ref Transform parent, ref byte stacksize, ref bool isStackable)
    {
        itemtype = gameObject;
        parent = _parent;
        stacksize = _quantity;
        isStackable = _isStackable;
    }

    public GameObject GetItem()
    {
        return gameObject;
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
        return _isStackable;
    }
}
