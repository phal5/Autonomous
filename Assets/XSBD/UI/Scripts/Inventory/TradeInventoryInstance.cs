using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.Progress;

public class TradeInventoryInstance : InventoryInstance
{
    [System.Serializable] public class Recipe
    {
        [SerializeField] string _name;
        [System.Serializable] public class ItemPair
        {
            public Transform _parent;
            public GameObject _item;
            public byte _quantity;
        }
        public ItemPair[] _ingredients;
        public ItemPair _result;
        public bool _stackable;
    }
    [SerializeField] Recipe _recipe;
    [SerializeField] TradeInventoryInstance _ingredientInventory;
    [SerializeField] TakeOnlySlot _returnSlot;
    [SerializeField] TradeSlot _preview;

    private void Start()
    {
        SetupTradeSlot();
        SetupInventorySlots();
    }

    void SetupTradeSlot()
    {
        _preview.SetSlotData(_recipe._result._item, _recipe._result._parent, _recipe._result._quantity, _recipe._stackable);
    }

    public bool Trade(byte index)
    {
        Recipe recipe = _recipe;
        InventorySlot[] slots = new InventorySlot[recipe._ingredients.Length];
        for(int i = 0; i < recipe._ingredients.Length; ++i)
        {
            InventorySlot slot = _ingredientInventory.SearchSlot(recipe._ingredients[i]._parent, recipe._ingredients[i]._item, recipe._ingredients[i]._quantity);
            if (slot != null)
            {
                slots[i] = slot;
            }
            else
            {
                return false;
            }
        }
        for (int i = 0; i < recipe._ingredients.Length; ++i)
        {
            slots[i].SetQuantity((byte)(slots[i].GetSlotQuantity() - recipe._ingredients[i]._quantity));
        }
        if(_returnSlot.GetSlotQuantity() == 0)
        {
            _returnSlot.SetSlotData(recipe._result._item, recipe._result._parent, recipe._result._quantity, recipe._stackable);
        }
        else
        {
            _returnSlot.SetQuantity((byte)(_returnSlot.GetSlotQuantity() + recipe._result._quantity));
        }
        return true;
    }

    public void TestTrade()
    {
        Debug.Log(Trade(0));
    }
}