using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInventoryInstance : InventoryInstance
{
    [SerializeField] byte[] _foodIndexes;
    // Start is called before the first frame update
    void Start()
    {
        SetCamera();
        SetupInventorySlots();
    }

    public bool Feed(byte amount, Vector3 position)
    {
        return RawInstantiate(SearchGivenFoodSlot(), amount, position, true);
    }

    bool RawInstantiate(InventorySlot slot, byte createAmount, Vector3 position, bool decrement = true)
    {
        if(slot != null)
        {
            if (decrement)
            {
                for (int i = 0; i < createAmount && slot.GetSlotQuantity() > 0; ++i)
                {
                    Instantiate(slot.GetSlotItem(), position, Random.rotation, slot.GetSlotParentData().GetParent());
                    slot.SetQuantity((byte)(slot.GetSlotQuantity() - 1));
                }
            }
            else
            {
                for (int i = 0; i < createAmount && slot.GetSlotQuantity() > 0; ++i)
                {
                    Instantiate(slot.GetSlotItem(), position, Random.rotation, slot.GetSlotParentData().GetParent());
                }
            }
            if (slot.GetSlotQuantity() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    InventorySlot SearchGivenFoodSlot()
    {
        foreach(InventorySlot slot in _slots)
        {
            foreach(Transform parent in FoodManager.GetFoodParents(_foodIndexes))
            {
                if (slot.GetSlotQuantity() > 0 && slot.GetSlotParentData().GetParent() == parent)
                {
                    return slot;
                }
            }
        }
        return null;
    }

    InventorySlot SearchAllFoodSlot()
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.GetSlotParentData().GetParent().parent.TryGetComponent<FoodManager>(out _))
            {
                return slot;
            }
        }
        return null;
    }
}
