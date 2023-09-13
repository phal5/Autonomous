using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feeder : MonoBehaviour
{
    [SerializeField] MachineInventoryInstance _inventory;
    [SerializeField] byte _amount = 3;

    public void Feed()
    {
        _inventory.Feed(_amount, transform.position);
    }
}
