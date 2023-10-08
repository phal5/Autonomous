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
        if(SearchGivenFoodSlot() != null)
        {
            Debug.Log("Yep");
        }
        return RawInstantiate(SearchGivenFoodSlot(), amount, position, true , true);
    }

    bool RawInstantiate(InventorySlot slot, byte createAmount, Vector3 position, bool decrement = true, bool deitemize = false)
    {
        GameObject gameObject = null;
        if(slot != null)
        {
            bool isDeployer = slot.GetSlotItem().TryGetComponent<Deployer>(out Deployer deployer);
            for (int i = 0; i < createAmount && slot.GetSlotQuantity() > 0; ++i)
            {
                gameObject = Instantiate
                    (
                        (isDeployer) ? deployer.GetDeployedObject() : slot.GetSlotItem(),
                        position, Random.rotation, slot.GetSlotParentData().GetParent()
                    );
                
                if (decrement) slot.SetQuantity((byte)(slot.GetSlotQuantity() - 1));
                if (deitemize) foreach (Item item in gameObject.GetComponents<Item>()) Destroy(item);
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
