using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DeleteGameObjectOfName : MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] bool _delete;

    // Update is called once per frame
    void Update()
    {
        if (_delete)
        {
            DeleteByName(transform);
            _delete = false;
        }
    }

    void DeleteByName(Transform _transform)
    {
        foreach (Transform child in _transform)
        {
            DeleteByName(child);
        }
        if (_transform.gameObject.name == _name)
        {
            foreach(Transform child in _transform)
            {
                child.parent = _transform.parent;
            }
            DestroyImmediate(_transform.gameObject);
            return;
        }
    }
}
