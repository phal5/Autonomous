using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryInstance : MonoBehaviour
{
    [SerializeField] protected InventorySlot[] _slots;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupInventorySlots();
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

    public void MoveToEmptySlot(GameObject item, Transform parent, byte quantity, bool stackable = true)
    {
        byte max = InventoryManager.GetMaxQuantity();
        InventorySlot slot = null;
        if (stackable)
        {
            slot = SearchStackableSlot(parent, item);
        }
        if (slot != null)
        {
            byte sum = (byte)(slot.GetSlotQuantity() + quantity);
            if (sum > max)
            {
                slot.SetQuantity(max);
                MoveToEmptySlot(item, parent, (byte)(sum - max));
            }
            else
            {
                slot.SetQuantity(sum);
            }
        }
        else
        {
            slot = SearchEmptySlot();
            if (slot != null)
            {
                if (quantity > max)
                {
                    slot.SetSlotData(item, parent, max, stackable);
                    MoveToEmptySlot(item, parent, (byte)(quantity - max));
                }
                else
                {
                    slot.SetSlotData(item, parent, quantity, stackable);
                }
            }
            else
            {
                InventoryManager.SetManagerData(item, parent, quantity, true);
                InventoryManager.ForceInstantiate();
            }
        }
    }

    protected InventorySlot SearchSlot(Transform parent, GameObject item, byte quantity = 1)
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.GetSlotQuantity() >= quantity && slot.GetSlotParent() == parent && slot.GetSlotItem() == item)
            {
                return slot;
            }
        }
        return null;
    }

    InventorySlot SearchStackableSlot(Transform parent, GameObject item, byte quantity = 1)
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.GetSlotQuantity() >= quantity && slot.GetSlotParent() == parent && slot.GetSlotItem() == item && slot.GetSlotStackable())
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
}
