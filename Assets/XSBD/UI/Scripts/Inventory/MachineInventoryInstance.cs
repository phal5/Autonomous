using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInventoryInstance : InventoryInstance
{
    // Start is called before the first frame update
    void Start()
    {
        SetupInventorySlots();
    }

    public bool RawInstantiate(GameObject item, Transform parent, ref byte originalQuantity, byte createAmount, Vector3 position)
    {
        for(int i = 0; i < createAmount && originalQuantity > 0; ++i)
        {
            Instantiate(item, position, Random.rotation, parent);
            --originalQuantity;
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
}
