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
            public ParentData _parentData;
            public GameObject _item;
            public byte _quantity;
        }
        public ItemPair[] _ingredients;
        public ItemPair _result;
        public bool _stackable;
    }
    [SerializeField] protected Recipe _recipe;
    [SerializeField] TakeOnlySlot _returnSlot;
    [SerializeField] TradeSlot _result;
    [SerializeField] Slot[] _ingredientPreview;

    private void Start()
    {
        //SetCamera();
        SetupTradeSlot();
        SetupInventorySlots();
    }

    protected void SetupTradeSlot()
    {
        _result.SetSlotData(_recipe._result._item, _recipe._result._parentData, _recipe._result._quantity, _recipe._stackable);
        for (int i = 0; i < _ingredientPreview.Length; ++i)
        {
            if(i < _recipe._ingredients.Length)
            {
                Recipe.ItemPair ingredient = _recipe._ingredients[i];
                _ingredientPreview[i].SetSlotData(ingredient._item, ingredient._parentData, ingredient._quantity, true);
            }
            else
            {
                _ingredientPreview[i].SetSlotData(null, null, 0, true);
            }
        }
    }

    public bool Trade(byte index)
    {
        if(_returnSlot.GetSlotQuantity() > 0 && !_returnSlot.GetSlotStackable())
        {
            return false;
        }

        Recipe recipe = _recipe;
        InventorySlot[] slots = new InventorySlot[recipe._ingredients.Length];
        for(int i = 0; i < recipe._ingredients.Length; ++i)
        {
            InventorySlot slot = SearchSlot(
                recipe._ingredients[i]._parentData,
                recipe._ingredients[i]._item,
                recipe._ingredients[i]._quantity);
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
            _returnSlot.SetSlotData(recipe._result._item, recipe._result._parentData, recipe._result._quantity, recipe._stackable);
        }
        else
        {
            _returnSlot.SetQuantity((byte)(_returnSlot.GetSlotQuantity() + recipe._result._quantity));
        }
        return true;
    }
}