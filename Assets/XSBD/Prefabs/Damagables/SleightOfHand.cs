using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleightOfHand : MonoBehaviour
{
    [SerializeField] GameObject _item;
    [SerializeField] ParentData _parent;
    [SerializeField] byte _quantity;
    [SerializeField] bool _stackable;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.GetInventoryInstance().MoveToEmptySlot(_item, _parent, _quantity, _stackable);
        Destroy(gameObject);
    }
}
