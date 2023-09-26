using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TakeOnlySlot : Slot
{
    bool _dragging = false;

    private void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemPosition();
    }

    protected void UpdateItemPosition()
    {
        if (_dragging && _itemInstance)
        {
            _itemInstance.position = InventoryManager.GetCamera().ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 2);
        }
    }

    public void MouseDownEvent()
    {
        if (_quantity != 0)
        {
            _dragging = true;
            InventoryManager.SetManagerData(_item, _parentData, _quantity, _stackable);
            Clear();
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
        _parentData = null;
        _quantity = 0;
        _stackable = true;
    }

    //---

    public GameObject GetSlotItem()
    {
        return _item;
    }

    public ParentData GetSlotParentData()
    {
        return _parentData;
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
