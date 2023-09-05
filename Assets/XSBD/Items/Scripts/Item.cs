using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Transform _transform;
    byte _stacksize;
    bool _isStackable;

    public void GetItemData(ref GameObject itemtype, ref Transform transform, ref byte stacksize, ref bool isStackable)
    {
        itemtype = gameObject;
        transform = _transform;
        stacksize = _stacksize;
        isStackable = _isStackable;
    }
}
