using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable] public class InventorySlot : MonoBehaviour
{
    [SerializeField] InventoryInstance _inventory;
    [SerializeField] GameObject _item;
    [SerializeField] Transform _parent;
    [SerializeField] byte _quantity = 0;
    [SerializeField] bool _stackable;

    bool _pull = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(_pull && InventoryManager.GetMove())
        {
            Pull();
        }
    }

    void Pull()
    {
        if (_quantity == 0)
        {
            _item = InventoryManager.GetItem();
            _parent = InventoryManager.GetItemParent();
            _quantity = InventoryManager.GetQuantity();
            _stackable = InventoryManager.GetStackability();
            InventoryManager.SetMove(false);
        }
        else if (_stackable && InventoryManager.GetStackability() && _parent == InventoryManager.GetItemParent())
        {
            _quantity += InventoryManager.GetQuantity();
            if (_quantity > 64)
            {
                if (_inventory.MoveToEmptySlot())
                {
                    InventoryManager.SetMove(false);
                }
            }
        }
    }

    public void MouseUpEvent()
    {
        if (_quantity != 0)
        {
            InventoryManager.SetItem(_item);
            InventoryManager.SetItemParent(_parent);
            InventoryManager.SetQuantity(_quantity);
            InventoryManager.SetStackability(_stackable);
            _item = null;
            _parent = null;
            _quantity = 0;
            _stackable = true;
            InventoryManager.SetMove(true);
        }
    }

    public void MouseEnterEvent()
    {
        Debug.Log("Yup");
        _pull = true;
        InventoryManager.SetStale(false);
    }

    public void MouseExitEvent()
    {
        _pull = false;
    }
    
    private void OnMouseUp()
    {
        MouseUpEvent();
    }

    private void OnMouseEnter()
    {
        MouseEnterEvent();
    }

    private void OnMouseExit()
    {
        MouseExitEvent();
    }

}
