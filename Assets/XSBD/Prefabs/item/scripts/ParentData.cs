using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable] public class ParentData
{
    enum Parent { ANIMAL, FOOD, NONE }

    [SerializeField] Parent _type = Parent.NONE;
    [SerializeField] byte _index = 0;

    Transform _parent = null;
    bool _parentSet = false;

    public ParentData()
    {
        _type = Parent.NONE;
        _index = 0;
        _parent = null;
        _parentSet = false;
    }

    Transform DecideParent()
    {
        Transform parent = null;
        switch (_type)
        {
            case Parent.ANIMAL:
                parent = AnimalManager.GetAnimalParent(_index);
                break;

            case Parent.FOOD:
                parent = FoodManager.GetFoodParent(_index);
                break;
        }
        _parent = parent;
        _parentSet = true;
        return _parent;
    }

    public Transform GetParent()
    {
        if (!_parentSet)
        {
            return DecideParent();
        }
        return _parent;
    }

    public GameObject InstantiateAsChild(GameObject gameObject, Vector3 position, quaternion rotation)
    {
        return MonoBehaviour.Instantiate(gameObject, position, rotation, GetParent());
    }

    public GameObject InstantiateRaycast(GameObject gameObject, Vector3 position, quaternion rotation)
    {
        float maxDimension = 0;
        if(gameObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            maxDimension = renderer.bounds.max.magnitude;
        }
        Physics.Raycast(position + Vector3.up * maxDimension, Vector3.down, out RaycastHit hit);
        return MonoBehaviour.Instantiate(gameObject, hit.point, rotation, GetParent());
    }

    public static ParentData zero()
    {
        ParentData data = new ParentData();
        data._type = Parent.NONE;
        data._index = 0;
        data._parent = null;
        data._parentSet = false;
        return data;
    }

    public bool Is(ParentData parentData)
    {
        if (parentData._type == _type && parentData._index == _index) return true;
        else return false;
    }

    public int GetTypeIndex()
    {
        return (int)_type;
    }
}
