using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryInstance : MonoBehaviour
{
    [SerializeField] protected InventorySlot[] _slots;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupInventorySlots();
    }

    protected void SetCamera()
    {
        SearchForCanvasInParents(transform).worldCamera = Camera.main;
    }

    Canvas SearchForCanvasInParents(Transform transform)
    {
        if(transform.parent)
        {
            if (transform.parent.TryGetComponent<Canvas>(out Canvas canvas))
            {
                return canvas;
            }
            else
            {
                return SearchForCanvasInParents(transform.parent);
            }
        }
        else
        {
            return null;
        }
    }

    protected void SetupInventorySlots()
    {
        List<InventorySlot> slots = new List<InventorySlot>();
        InventorySlot slot;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<InventorySlot>(out slot))
            {
                slots.Add(slot);
                slot.SetInventoryInstance(this);
            }
        }
        _slots = slots.ToArray();
    }

    protected InventorySlot SearchSlot(ParentData parentData, GameObject item, byte quantity = 1)
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.GetSlotQuantity() >= quantity && slot.GetSlotParentData().GetParent() == parentData.GetParent() && slot.GetSlotItem() == item)
            {
                return slot;
            }
        }
        return null;
    }

    InventorySlot SearchStackableSlot(ParentData parentData, GameObject item, byte quantity = 1)
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.GetSlotQuantity() >= quantity && slot.GetSlotParentData() != null && slot.GetSlotParentData().GetParent() == parentData.GetParent() && slot.GetSlotItem() == item && slot.GetSlotStackable())
            {
                return slot;
            }
        }
        return null;
    }

    InventorySlot SearchEmptySlot()
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.GetSlotQuantity() == 0)
            {
                return slot;
            }
        }
        return null;
    }

    public bool MoveToEmptySlot(GameObject item, ParentData parentData, byte quantity, bool stackable = true)
    {
        byte max = InventoryManager.GetMaxQuantity();
        InventorySlot slot = null;
        if (stackable)
        {
            slot = SearchStackableSlot(parentData, item);
        }
        if (slot != null)
        {
            byte sum = (byte)(slot.GetSlotQuantity() + quantity);
            if (sum > max)
            {
                slot.SetQuantity(max);
                MoveToEmptySlot(item, parentData, (byte)(sum - max));
            }
            else
            {
                slot.SetQuantity(sum);
            }
            return true;
        }
        else
        {
            slot = SearchEmptySlot();
            if (slot != null)
            {
                if (quantity > max)
                {
                    slot.SetSlotData(item, parentData, max, stackable);
                    MoveToEmptySlot(item, parentData, (byte)(quantity - max));
                }
                else
                {
                    slot.SetSlotData(item, parentData, quantity, stackable);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
