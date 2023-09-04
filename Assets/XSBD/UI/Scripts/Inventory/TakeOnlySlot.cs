using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TakeOnlySlot : Slot
{
    bool _dragging = false;

    // Update is called once per frame
    void Update()
    {
        UpdateItemPosition();
    }

    protected void UpdateItemPosition()
    {
        if (_dragging && _itemInstance)
        {
            _itemInstance.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 2);
        }
    }

    public void MouseDownEvent()
    {
        if (_quantity != 0)
        {
            _dragging = true;
            InventoryManager.SetManagerData(_item, _parent, _quantity, _stackable);
            Clear();
            InventoryManager.SetSlot(this);
            if (_itemInstance)
            {
                SetLayerInChildren(_itemInstance, 7);
            }
        }
    }

    public void MouseUpEvent()
    {
        if (_dragging)
        {
            _dragging = false;
            SetText();
            InventoryManager.SetStale(false);
            if (InventoryManager.GetQuantity() != 0)
            {
                InventoryManager.SetMove(true);
            }
            if (_itemInstance)
            {
                Destroy(_itemInstance.gameObject);
            }
        }
    }

    //---

    protected void Clear()
    {
        _item = null;
        _parent = null;
        _quantity = 0;
        _stackable = true;
    }

    //---

    public GameObject GetSlotItem()
    {
        return _item;
    }

    public Transform GetSlotParent()
    {
        return _parent;
    }

    public bool GetSlotStackable()
    {
        if (_stackable && _quantity < InventoryManager.GetMaxQuantity())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public byte GetSlotQuantity()
    {
        return _quantity;
    }

    public void SetQuantity(byte quantity)
    {
        _quantity = quantity;
        SetText();
        if (_quantity <= 0 && _itemInstance)
        {
            Destroy(_itemInstance.gameObject);
        }
    }
}
