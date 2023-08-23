using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryInstance : MonoBehaviour
{
    [SerializeField] InventorySlot[] _slots;
    // Start is called before the first frame update
    void Start()
    {
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

    public bool MoveToEmptySlot()
    {
        bool successful = false;
        //dummy
        return successful;
    }
}
