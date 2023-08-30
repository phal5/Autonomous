using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryInstance : MonoBehaviour
{
    [SerializeField] InventorySlot[] _slots;
    Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.current;
        List<InventorySlot> slots = new List<InventorySlot>();
        InventorySlot slot;
        foreach (Transform child in transform)
        {
            if(child.TryGetComponent<InventorySlot>(out slot))
            {
                slots.Add(slot);
                slot.SetInventoryInstance(this);
            }
        }
        _slots = slots.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToEmptySlot(Transform parent, GameObject item, byte quantity)
    {
        byte max = InventoryManager.GetMaxQuantity();
        InventorySlot slot = SearchSlot(parent, item);
        if(slot != null)
        {
            byte sum = (byte)(slot.GetSlotQuantity() + quantity);
            if (sum > max)
            {
                slot.SetQuantity(max);
                MoveToEmptySlot(parent, item, (byte)(sum - max));
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
                    slot.SetQuantity((byte)(quantity));
                    MoveToEmptySlot(parent, item, (byte)(quantity - max));
                }
                else
                {
                    slot.SetSlotData(item, parent, quantity, true);
                }
            }
            else
            {
                InventoryManager.SetManagerData(item, parent, quantity, true);
                InventoryManager.ForceInstantiate();
            }
        }
    }

    InventorySlot SearchSlot(Transform parent, GameObject item)
    {
        foreach(InventorySlot slot in _slots)
        {
            if(slot.GetSlotParent() == parent && slot.GetSlotItem() == item && slot.GetSlotStackable())
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
