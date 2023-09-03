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
        [System.Serializable] public class ItemPair
        {
            [SerializeField] public Transform _parent;
            [SerializeField] public GameObject _item;
            [SerializeField] public byte _quantity;
        }
        public ItemPair[] _items;
        public ItemPair _result;
        public bool _stackable;
    }
    [SerializeField] Recipe[] _recipes;
    [SerializeField] TradeInventoryInstance _ingredientInventory;
    [SerializeField] InventoryInstance _returnInventory;
    bool _trade;

    // Start is called before the first frame update
    public bool Trade(byte _recipeIndex)
    {
        Recipe recipe = _recipes[_recipeIndex];
        InventorySlot[] slots = new InventorySlot[recipe._items.Length];
        for(int i = 0; i < recipe._items.Length; ++i)
        {
            InventorySlot slot = _ingredientInventory.SearchSlot(recipe._items[i]._parent, recipe._items[i]._item, recipe._items[i]._quantity);
            if (slot != null)
            {
                slots[i] = slot;
            }
            else
            {
                return false;
            }
        }
        for (int i = 0; i < recipe._items.Length; ++i)
        {
            slots[i].SetQuantity((byte)(slots[i].GetSlotQuantity() - recipe._items[i]._quantity));
        }
        _returnInventory.MoveToEmptySlot(recipe._result._item, recipe._result._parent, recipe._result._quantity, recipe._stackable);
        return true;
    }
}
