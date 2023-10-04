using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode] public class ColRowInstancerRandomized : MonoBehaviour
{
    [SerializeField] Transform _thisEnd;
    [SerializeField] Transform _thatEnd;
    [Space(10f)]
    [SerializeField] Transform _parent;
    [SerializeField] GameObject _instancee;
    [SerializeField] float _distance;
    [Space(10f)]
    [Tooltip("Not necessary")][SerializeField] float _manualDimension = 0.1f;
    [Tooltip("Not necessary")][SerializeField] GameObject _dimensionsRef;
    [Space(10f)]
    [SerializeField] bool _instantiate = false;
    [SerializeField] bool _undo = false;
    [SerializeField] bool _clearAll = false;

    List<List<GameObject>> _installList = new List<List<GameObject>>();

    // Update is called once per frame
    void Update()
    {
        if (_instantiate)
        {
            _instantiate = false;
            InstantiateColRow();
        }
        if (_undo)
        {
            _undo = false;
            Undo();
        }
        if (_clearAll)
        {
            _clearAll = false;
            foreach(List<GameObject> list in _installList)
            {
                foreach (GameObject gameObject in list)
                {
                    DestroyImmediate(gameObject);
                }
            }
            _installList.Clear();
            Debug.Log("Cleared All Objects!");
        }
    }

    void InstantiateColRow()
    {
        float xLength = (_thisEnd.position.x - _thatEnd.position.x);
        float zLength = (_thisEnd.position.z - _thatEnd.position.z);
        int x = 1;
        int z = 1;
        Vector3 dimensions = _manualDimension * Vector3.one;
        List<GameObject> list = new List<GameObject>();
        if(_dimensionsRef != null && _dimensionsRef.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
        {
            dimensions = renderer.bounds.size;
        }
        dimensions += Vector3.one * _distance;
        if(dimensions.x > 0)
        {
            x = (int)(xLength / dimensions.x);
            if (x < 0)
            {
                x = -x;
                dimensions.x = -dimensions.x;
            }
        }
        if (dimensions.z > 0)
        {
            z = (int)(zLength / dimensions.z);
            if (z < 0)
            {
                z = -z;
                dimensions.z = -dimensions.z;
            }
        }
        for(int i = 0; i < x; i++)
        {
            for(int k = 0; k < z; k++)
            {
                list.Add
                    (
                        Instantiate
                        (
                            _instancee,
                            (dimensions.x * i - _thisEnd.position.x + dimensions.x * 0.5f - _distance * Random.value) * Vector3.right
                            + (dimensions.z * k - _thisEnd.position.z + dimensions.z * 0.5f - _distance * Random.value) * Vector3.forward
                            + transform.position.y * Vector3.up,
                            Quaternion.Euler(Vector3.zero),
                            _parent
                        )
                    );
            }
        }
        _installList.Add(list);
    }

    void Undo()
    {
        if(_installList.Count > 0)
        {
            foreach (GameObject gameObject in _installList[^1])
            {
                DestroyImmediate(gameObject);
            }
            _installList.Remove(_installList[^1]);
        }
        else
        {
            Debug.LogError("Nothing installed yet!");
        }
    }
}
