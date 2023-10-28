using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform player;
    static Transform _player;

    [SerializeField] Camera camera_;
    static Camera _camera;
    static GameObject _Item;
    static ParentData _ParentData;
    static InventorySlot _Slot;

    static byte _MaxQuantity = 64;
    static byte _Quantity;
    static bool _stale = false;
    static bool _Move;
    static bool _Stackable;

    // Start is called before the first frame update
    void Start()
    {
        _player = player;
        _camera = camera_;

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
        if (_Item.TryGetComponent<Deployer>(out Deployer deployer) && deployer.GetDeployedObject().TryGetComponent<MachineDeployer>(out _))
        {
            Instantiate(_Item, _player.position + Vector3.up, Quaternion.Euler(Vector3.zero), (_ParentData == null) ? null : _ParentData.GetParent());
            _Slot.SetSlotData(_Item, _ParentData, --_Quantity, _Stackable);
        }
        else for (int i = 0; i < _Quantity; i++)
        {
            Instantiate(_Item, _player.position + Vector3.up, Quaternion.Euler(Vector3.zero), (_ParentData == null) ? null : _ParentData.GetParent());
        }

        _Move = false;
        _stale = false;
        Clear();
    }

    public static void SetManagerData(GameObject Item, ParentData ParentData, byte Quantity, bool Stackability)
    {
        _Item = Item;
        _ParentData = ParentData;
        _Quantity = Quantity;
        _Stackable = Stackability;
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
        _ParentData = null;
        _Slot = null;
        _Quantity = 0;
        _Stackable = false;
    }

    public static void ForceInstantiate()
    {
        InstantiateItem();
    }

    //---

    public static Camera GetCamera()
    {
        return _camera;
    }

    public static GameObject GetItem()
    {
        return _Item;
    }

    public static ParentData GetItemParentData()
    {
        return _ParentData;
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

    public static InventorySlot GetSlot()
    {
        return _Slot;
    }
}
