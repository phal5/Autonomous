using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Deployer : MonoBehaviour
{
    [SerializeField] GameObject _deployObject;
    [SerializeField] bool _randomizeRotation = true;

    ParentData _parentData;
    bool _parentSet;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_randomizeRotation)
        {
            Deploy(transform.position, Random.rotation);
        }
        else
        {
            Deploy(transform.position, Quaternion.Euler(Vector3.zero));
        }
    }

    protected bool Deploy(Vector3 position, Quaternion rotation)
    {
        GameObject deployObject = Instantiate(_deployObject, position, rotation, transform.parent);
        if (deployObject != null)
        {
            if(deployObject.TryGetComponent<Item>(out Item item))
            {
                deployObject.transform.parent = item.GetParentData().GetParent();
            }
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public ParentData GetParentData()
    {
        if (!_parentSet)
        {
            _parentSet = true;
            if (_deployObject.TryGetComponent<Item>(out Item item))
            {
                _parentData = item.GetParentData();
            }
        }
        return _parentData;
    }
}
