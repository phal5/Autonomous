using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[System.Serializable] public class InventorySlot : MonoBehaviour
{
    [SerializeField] InventoryInstance _inventory;
    [SerializeField] GameObject _item;
    [SerializeField] Transform _parent;
    [SerializeField] byte _quantity = 0;
    [SerializeField] bool _stackable;

    [SerializeField] Transform _itemInstance;

    [SerializeField] Vector3 _scaleDivisor;

    bool _pull = false;
    bool _dragging = false;

    static bool _interacting = false;
    bool _interactingWthis = false;

    // Start is called before the first frame update
    void Start()
    {
        if(_quantity != 0)
        {
            _scaleDivisor.x = 1f / transform.lossyScale.x;
            _scaleDivisor.y = 1f / transform.lossyScale.y;
            _scaleDivisor.z = 1f / transform.lossyScale.z;
            InstanceItem();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_pull && InventoryManager.GetMove())
        {
            Pull();
        }
        if (_dragging)
        {
            _itemInstance.position = InventoryManager.GetCamera().ScreenToWorldPoint(Input.mousePosition + Vector3.forward);
        }
    }

    void Pull()
    {
        if (_itemInstance)
        {
            Destroy(_itemInstance.gameObject);
        }
        InventoryManager.SetMove(false);
        if (_quantity == 0)
        {
            GetManagerData();
        }
        else if (_stackable && InventoryManager.GetStackability() && _parent == InventoryManager.GetItemParent())
        {
            _quantity += InventoryManager.GetQuantity();
            if (_quantity > 64)
            {
                if (!_inventory.MoveToEmptySlot())
                {

                }
            }
            
        }
        else
        {
            InventoryManager.GetSlot().SetSlotData(_item, _parent, _quantity, _stackable);
            GetManagerData();
            InventoryManager.Clear();
        }
        InventoryManager.SetSlot(null);
        InstanceItem();
    }

    public void MouseDownEvent()
    {
        if(_quantity != 0 && !_interacting)
        {
            _interacting = true;
            _interactingWthis = true;
            InventoryManager.SetManagerData(_item, _parent, _quantity, _stackable);
            Clear();
            InventoryManager.SetSlot(this);
            _dragging = true;
        }
    }

    public void MouseUpEvent()
    {
        if (_interactingWthis)
        {
            _interacting = false;
            _interactingWthis = false;
            _dragging = false;
            if (InventoryManager.GetQuantity() != 0)
            {
                InventoryManager.SetMove(true);
            }
            if (_itemInstance)
            {
                Destroy(_itemInstance.gameObject);
            }
        }
    }

    public void MouseEnterEvent()
    {
        _pull = true;
        InventoryManager.SetStale(false);
    }

    public void MouseExitEvent()
    {
        _pull = false;
    }

    //---
    
    private void OnMouseUp()
    {
        MouseUpEvent();
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MouseDownEvent();
        }
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MouseEnterEvent();
        }
    }

    private void OnMouseExit()
    {
        MouseExitEvent();
    }

    //---

    private void GetManagerData()
    {
        _item = InventoryManager.GetItem();
        _parent = InventoryManager.GetItemParent();
        _quantity = InventoryManager.GetQuantity();
        _stackable = InventoryManager.GetStackability();
    }

    private void Clear()
    {
        _item = null;
        _parent = null;
        _quantity = 0;
        _stackable = true;
    }

    private void InstanceItem()
    {
        if (_itemInstance)
        {
            Destroy(_itemInstance.gameObject);
        }

        _itemInstance = Instantiate(_item, transform.position - Vector3.forward * 10, Quaternion.Euler(Vector3.up * 180), transform).transform;

        if (_scaleDivisor == Vector3.zero)
        {
            _scaleDivisor.x = 1f / transform.lossyScale.x;
            _scaleDivisor.y = 1f / transform.lossyScale.y;
            _scaleDivisor.z = 1f / transform.lossyScale.z;
        }

        _itemInstance.localScale = _scaleDivisor * 0.7f;

        SetLayerInChildren(_itemInstance, 7);
        ClearColliders(_itemInstance);
        ClearBehaviours(_itemInstance);
        ClearNavAgents(_itemInstance);
        ClearRigidBody(_itemInstance);
    }

    private void SetLayerInChildren(Transform transform, int layer)
    {
        transform.gameObject.layer = layer;
        foreach (Transform child in transform)
        {
            SetLayerInChildren(child, layer);
        }
    }

    void ClearColliders(Transform transform)
    {
        foreach(Collider collider in transform.GetComponents<Collider>())
        {
            Destroy(collider);
        }
        foreach(Transform child in transform)
        {
            ClearColliders(child);
        }
    }

    void ClearBehaviours(Transform transform)
    {
        foreach (MonoBehaviour behaviour in transform.GetComponents<MonoBehaviour>())
        {
            behaviour.enabled = false;
        }
        foreach (Transform child in transform)
        {
            ClearBehaviours(child);
        }
    }

    void ClearNavAgents(Transform transform)
    {
        foreach (NavMeshAgent navMeshAgent in transform.GetComponents<NavMeshAgent>())
        {
            Destroy(navMeshAgent);
        }
        foreach (Transform child in transform)
        {
            ClearNavAgents(child);
        }
    }

    void ClearRigidBody(Transform transform)
    {
        foreach(Rigidbody rigidbody in transform.GetComponents<Rigidbody>())
        {
            Destroy(rigidbody);
        }
        foreach(Transform child in transform)
        {
            ClearRigidBody(child);
        }
    }

    //---

    public GameObject GetSlotItem()
    {
        return _item;
    }

    public Transform GetSlotParent()
    {
        return _parent;
    }

    public void SetSlotData(GameObject item, Transform parent, byte quantity, bool stackable)
    {
        _item = item;
        _parent = parent;
        _quantity = quantity;
        _stackable = stackable;
        InstanceItem();
    }
}
