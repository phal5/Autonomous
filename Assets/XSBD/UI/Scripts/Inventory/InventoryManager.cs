using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform __player;
    static Transform _player;

    static GameObject _Item;
    static Transform _Parent;
    static InventorySlot _Slot;

    static byte _MaxQuantity = 64;
    static byte _Quantity;
    static bool _stale = false;
    static bool _Move;
    static bool _Stackable;

    // Start is called before the first frame update
    void Start()
    {
        _player = __player;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Move)
        {
            if (_stale)
            {
                InstantiateItem();
            }
            else
            {
                _stale = true;
            }
        }
    }

    protected static void InstantiateItem()
    {
        for(int i = 0; i < _Quantity; i++)
        {
            Instantiate(_Item, _player.position + Vector3.up, Random.rotation, _Parent);
        }
        _Move = false;
        _stale = false;
        Clear();
    }

    public static void SetManagerData(GameObject Item, Transform Parent, byte Quantity, bool stackability)
    {
        _Item = Item;
        _Parent = Parent;
        _Quantity = Quantity;
        _Stackable = stackability;
    }

    public static void SetMove(bool move)
    {
        _Move = move;
    }

    public static void SetStale(bool stale)
    {
        _stale = stale;
    }

    public static void SetSlot(InventorySlot slot)
    {
        _Slot = slot;
    }

    //---

    public static void Clear()
    {
        _Item = null;
        _Parent = null;
        _Slot = null;
        _Quantity = 0;
        _Stackable = false;
    }

    public static void ForceInstantiate()
    {
        InstantiateItem();
    }

    //---

    public static GameObject GetItem()
    {
        return _Item;
    }

    public static Transform GetItemParent()
    {
        return _Parent;
    }

    public static byte GetMaxQuantity()
    {
        return _MaxQuantity;
    }

    public static byte GetQuantity()
    {
        return _Quantity;
    }

    public static bool GetStackability()
    {
        return _Stackable;
    }

    public static bool GetMove()
    {
        return _Move;
    }

    public static TakeOnlySlot GetSlot()
    {
        return _Slot;
    }
}
