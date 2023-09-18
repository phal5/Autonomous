using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SpontaneousTrader : TradeInventoryInstance
{
    [System.Serializable] class RecipeList
    {
        public Recipe _recipe;
        public byte _probability;
    }
    [SerializeField] RecipeList[] _recipes;
    float _divisor = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach(RecipeList recipe in _recipes)
        {
            _divisor += recipe._probability;
        }
        _divisor = 1 / _divisor;
    }

    public void ResetRecipe()
    {
        float index = Random.value;
        Recipe _Recipe = null;
        foreach(RecipeList recipe in _recipes)
        {
            if(recipe._probability * _divisor > index)
            {
                _Recipe = recipe._recipe;
                break;
            }
            else
            {
                index -= _divisor * recipe._probability;
            }
            Debug.Log(index);
        }
        if (_Recipe == null)
        {
            _Recipe = _recipes[^1]._recipe;
        }
        _recipe = _Recipe;
        SetupTradeSlot();
    }
}
