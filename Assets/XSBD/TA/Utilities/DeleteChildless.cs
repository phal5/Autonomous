using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DeleteChildless : MonoBehaviour
{
    [SerializeField] bool _firstHandChildrenOnly;
    [SerializeField] bool _delete;
    List<GameObject> _deleteList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (_delete)
        {
            _deleteList.Clear();
            if (_firstHandChildrenOnly) ListChildlessFirstHand();
            else ListChildless(transform);
            foreach (GameObject child in _deleteList)
            {
                DestroyImmediate(child);
            }
            _delete = false;
        }
    }

    void ListChildless(Transform _transform)
    {
        if (_transform.childCount == 0)
        {
            _deleteList.Add(_transform.gameObject);
        }
        else
        {
            foreach (Transform child in _transform)
            {
                ListChildless(child);
            }
        }
    }

    void ListChildlessFirstHand()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount == 0)
            {
                _deleteList.Add(child.gameObject);
            }
        }
    }
}
