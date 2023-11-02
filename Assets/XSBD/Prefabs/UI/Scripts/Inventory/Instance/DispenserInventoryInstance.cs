using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserInventoryInstance : InventoryInstance
{
    public void Dispend(Transform transform)
    {
        foreach(InventorySlot slot in _slots)
        {
            if(slot.GetSlotQuantity() > 0)
            {
                slot.SetQuantity((byte)(slot.GetSlotQuantity() - 1));
                Instantiate(slot.GetSlotItem(), transform.position, transform.rotation, slot.GetSlotParentData().GetParent());
            }
        }
    }
}
