using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[System.Serializable] public class InventorySlot : MonoBehaviour
{
    [SerializeField] GameObject _item;
    [SerializeField] Transform _parent;
    [SerializeField] byte _quantity = 0;
    [SerializeField] bool _stackable;

    TextMeshProUGUI _textMeshProUGUI;
    InventoryInstance _inventory;
    Transform _itemInstance;
    
    Vector3 _scaleDivisor;
    bool _pull = false;
    bool _dragging = false;

    private void Awake()
    {
        if (transform.parent)
        {
            SetLayerInChildren(transform, transform.parent.gameObject.layer);
        }
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        SetText();
    }

    // Start is called before the first frame update
    void Start()
    {
        _scaleDivisor.x = 1f / transform.lossyScale.x;
        _scaleDivisor.y = 1f / transform.lossyScale.y;
        _scaleDivisor.z = 1f / transform.lossyScale.z;
        if (_quantity != 0)
        {
            InstanceItem();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_pull && InventoryManager.GetMove())
        {
            Pull();
            InventoryManager.Clear();
        }
        if (_dragging)
        {
            _itemInstance.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 2);
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
            SolveOverload();
        }
        else if (_stackable && InventoryManager.GetStackability() && _parent == InventoryManager.GetItemParent() && _item == InventoryManager.GetItem())
        {
            _quantity += InventoryManager.GetQuantity();
            SolveOverload();
        }
        else
        {
            InventoryManager.GetSlot().SetSlotData(_item, _parent, _quantity, _stackable);
            GetManagerData();
            InventoryManager.Clear();
        }
        InventoryManager.SetSlot(null);
        SetText();
        InstanceItem();
    }

    public void MouseDownEvent()
    {
        if(_quantity != 0)
        {
            InventoryManager.SetManagerData(_item, _parent, _quantity, _stackable);
            Clear();
            InventoryManager.SetSlot(this);
            _dragging = true;
            SetLayerInChildren(_itemInstance, 7);
        }
    }

    public void MouseUpEvent()
    {
        _dragging = false;
        SetText();
        InventoryManager.SetStale(false);
        if (InventoryManager.GetQuantity() != 0)
        {
            InventoryManager.SetMove(true);
        }
        if (_itemInstance)
        {
            Destroy(_itemInstance.gameObject);
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

    private void OnMouseOver()
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

    private void SolveOverload()
    {
        byte maxQuantity = InventoryManager.GetMaxQuantity();
        if (_quantity > maxQuantity)
        {
            _inventory.MoveToEmptySlot(_item, _parent, (byte)(_quantity - maxQuantity));
            _quantity = maxQuantity;
        }
    }

    private void InstanceItem()
    {
        if (_itemInstance)
        {
            Destroy(_itemInstance.gameObject);
        }

        _itemInstance = Instantiate(_item, transform).transform;

        if (_scaleDivisor == Vector3.zero)
        {
            _scaleDivisor.x = 1f / transform.lossyScale.x;
            _scaleDivisor.y = 1f / transform.lossyScale.y;
            _scaleDivisor.z = 1f / transform.lossyScale.z;
        }

        SetLayerInChildren(_itemInstance, gameObject.layer);
        ClearColliders(_itemInstance);
        ClearBehaviours(_itemInstance);
        ClearNavAgents(_itemInstance);
        ClearRigidBody(_itemInstance);

        _itemInstance.localScale = _scaleDivisor * 0.7f;
        _itemInstance.position = transform.position - transform.forward * 0.5f;
        _itemInstance.rotation = transform.rotation * Quaternion.Euler(Vector3.up * 180);
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

    void SetText()
    {
        if(_textMeshProUGUI != null)
        {
            if (_quantity > 0)
            {
                _textMeshProUGUI.text = _quantity.ToString();
            }
            else
            {
                _textMeshProUGUI.text = "";
            }
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

    public bool GetSlotStackable()
    {
        if(_stackable && _quantity < InventoryManager.GetMaxQuantity())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public byte GetSlotQuantity()
    {
        return _quantity;
    }

    public void SetSlotData(GameObject item, Transform parent, byte quantity, bool stackable)
    {
        _item = item;
        _parent = parent;
        _quantity = quantity;
        _stackable = stackable;
        SetText();
        InstanceItem();
    }

    public void SetQuantity(byte quantity)
    {
        _quantity = quantity;
        SetText();
    }

    public void SetInventoryInstance(InventoryInstance inventoryInstance)
    {
        _inventory = inventoryInstance;
    }
}
