using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class ParentData
{
    enum Parent { ANIMAL, FOOD, NONE }
    
    [SerializeField] Parent _type = Parent.NONE;
    [SerializeField] byte _index;

    Transform _parent = null;
    bool _parentSet = false;

    Transform DecideParent()
    {
        Transform parent;
        switch (_type)
        {
            case Parent.ANIMAL:
                parent = AnimalManager.GetAnimalParent(_index);
                break;

            case Parent.FOOD:
                parent = FoodManager.GetFoodParent(_index);
                break;

            default:
                parent = null;
                break;
        }
        _parent = parent;
        _parentSet = true;
        return parent;
    }

    public Transform GetParent()
    {
        if (!_parentSet)
        {
            return DecideParent();
        }
        return _parent;
    }
}
