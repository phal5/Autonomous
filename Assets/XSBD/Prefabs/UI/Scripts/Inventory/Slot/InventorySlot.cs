using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[System.Serializable] public class InventorySlot : TakeOnlySlot
{
    InventoryInstance _inventory;

    bool _pull = false;

    private void Start()
    {
        SetText();
        SetScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (_pull && InventoryManager.GetMove())
        {
            Pull();
            InventoryManager.Clear();
        }
        UpdateItemPosition();
    }

    void Pull()
    {
        if (_itemInstance)
        {
            Destroy(_itemInstance.gameObject);
        }
        InventoryManager.SetMove(false);
        if (_quantity == 0)
        {
            GetManagerData();
            SolveOverload();
        }
        else if (_stackable && InventoryManager.GetStackability() && _parent == InventoryManager.GetItemParent() && _item == InventoryManager.GetItem())
        {
            _quantity += InventoryManager.GetQuantity();
            SolveOverload();
        }
        else
        {
            if (InventoryManager.GetSlot() != null)
            {
                InventoryManager.GetSlot().SetSlotData(_item, _parent, _quantity, _stackable);
                GetManagerData();
                InventoryManager.Clear();
            }
            else
            {
                _inventory.MoveToEmptySlot(
                    InventoryManager.GetItem(),
                    InventoryManager.GetItemParent(),
                    InventoryManager.GetQuantity(),
                    InventoryManager.GetStackability());
            }
        }
        InventoryManager.SetSlot(null);
        SetText();
        InstanceItem();
    }

    public void MouseEnterEvent()
    {
        _pull = true;
        InventoryManager.SetStale(false);
    }

    public void MouseExitEvent()
    {
        _pull = false;
    }

    //---

    private void OnMouseUp()
    {
        MouseUpEvent();
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MouseDownEvent();
        }
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MouseEnterEvent();
        }
    }

    private void OnMouseExit()
    {
        MouseExitEvent();
    }

    //---

    private void GetManagerData()
    {
        _item = InventoryManager.GetItem();
        _parent = InventoryManager.GetItemParent();
        _quantity = InventoryManager.GetQuantity();
        _stackable = InventoryManager.GetStackability();
    }

    private void SolveOverload()
    {
        byte maxQuantity = InventoryManager.GetMaxQuantity();
        if (_quantity > maxQuantity)
        {
            _inventory.MoveToEmptySlot(_item, _parent, (byte)(_quantity - maxQuantity));
            _quantity = maxQuantity;
        }
    }

    //---

    public void SetInventoryInstance(InventoryInstance inventoryInstance)
    {
        _inventory = inventoryInstance;
    }

    public void SetDisplay()
    {

    }
}
