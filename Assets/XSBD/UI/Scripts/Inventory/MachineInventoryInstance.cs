using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInventoryInstance : InventoryInstance
{
    [SerializeField] byte[] _foodIndexes;
    // Start is called before the first frame update
    void Start()
    {
        SetupInventorySlots();
    }

    public void FindAndInstantiate()
    {
        
    }

    public void Feed(byte amount, Vector3 position)
    {
        RawInstantiate(SearchGivenFoodSlot(), amount, position, true);
    }

    /*
    public bool RawInstantiate(GameObject item, Transform parent, ref byte originalQuantity, byte createAmount, Vector3 position, bool decrement = true)
    {
        if (decrement)
        {
            for (int i = 0; i < createAmount && originalQuantity > 0; ++i)
            {
                Instantiate(item, position, Random.rotation, parent);
                --originalQuantity;
            }
        }
        else
        {
            for (int i = 0; i < createAmount && originalQuantity > 0; ++i)
            {
                Instantiate(item, position, Random.rotation, parent);
            }
        }
        Debug.Log(createAmount);
        if(originalQuantity > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */

    bool RawInstantiate(InventorySlot slot, byte createAmount, Vector3 position, bool decrement = true)
    {
        if (decrement)
        {
            for (int i = 0; i < createAmount && slot.GetSlotQuantity() > 0; ++i)
            {
                Instantiate(slot.GetSlotItem(), position, Random.rotation, slot.GetSlotParent());
                slot.SetQuantity((byte)(slot.GetSlotQuantity() - 1));
            }
        }
        else
        {
            for (int i = 0; i < createAmount && slot.GetSlotQuantity() > 0; ++i)
            {
                Instantiate(slot.GetSlotItem(), position, Random.rotation, slot.GetSlotParent());
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

    InventorySlot SearchGivenFoodSlot()
    {
        foreach(InventorySlot slot in _slots)
        {
            foreach(Transform parent in FoodManager.GetFoodParents(_foodIndexes))
            {
                if (slot.GetSlotParent() == parent)
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
            if (slot.GetSlotParent().parent.TryGetComponent<FoodManager>(out _))
            {
                return slot;
            }
        }
        return null;
    }
}
