using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] GameObject _item;
    [SerializeField] byte _quantity;
    [SerializeField] bool _stackable;
    [Space(10f)]
    [SerializeField] ParentData _parentData;

    private void Start()
    {
        if(_parentData == null)
        {
            _parentData = new ParentData();
        }
        _parentData.GetParent();
    }

    public void GetItemData(out GameObject item, out ParentData parentData, out byte quantity, out bool stackable)
    {
        item = _item;
        parentData = _parentData;
        quantity = _quantity;
        stackable = _stackable;
    }

    public GameObject GetItem()
    {
        return _item;
    }

    public ParentData GetParentData()
    {
        return _parentData;
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
