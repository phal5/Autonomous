using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform _player;

    static GameObject _Item;
    static Transform _Parent;

    static byte _Quantity;
    static bool _stale = false;
    static bool _Move;
    static bool _Stackable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_stale);
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

    private void InstantiateItem()
    {
        Instantiate(_Item, _player.position + Vector3.up, Random.rotation, _Parent);
        _Move = false;
        _stale = false;
    }

    public static void SetItem(GameObject Item)
    {
        _Item = Item;
    }

    public static void SetItemParent(Transform parent)
    {
        if (parent)
        {
            _Parent = parent;
        }
    }

    public static void SetQuantity(byte quantity)
    {
        _Quantity = quantity;
    }

    public static void SetStackability(bool stackability)
    {
        _Stackable = stackability;
    }

    public static void SetMove(bool move)
    {
        _Move = move;
    }

    public static GameObject GetItem()
    {
        return _Item;
    }

    public static Transform GetItemParent()
    {
        return _Parent;
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
}
