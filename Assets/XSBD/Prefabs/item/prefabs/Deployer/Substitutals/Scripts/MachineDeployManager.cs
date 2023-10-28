using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineDeployManager : MonoBehaviour
{
    [SerializeField] Material deployable;
    [SerializeField] Material notDeployable;
    [Space(10f)]
    [SerializeField] GameObject pointerPrefab;
    [SerializeField] KeyCode xRotatePlus = KeyCode.I;
    [SerializeField] KeyCode xRotateMinus = KeyCode.K;
    [SerializeField] KeyCode yRotatePlus = KeyCode.J;
    [SerializeField] KeyCode yRotateMinus = KeyCode.L;
    [SerializeField] KeyCode cancel = KeyCode.Escape;

    static GameObject _pointerPrefab;
    static Material _deployable;
    static Material _notDeployable;
    static KeyCode _xRotatePlus;
    static KeyCode _xRotateMinus;
    static KeyCode _yRotatePlus;
    static KeyCode _yRotateMinus;
    static KeyCode _cancel;
    // Start is called before the first frame update
    void Start()
    {
        _pointerPrefab = pointerPrefab;
        _deployable = deployable;
        _notDeployable = notDeployable;
        _xRotatePlus = xRotatePlus;
        _xRotateMinus = xRotateMinus;
        _yRotatePlus = yRotatePlus;
        _yRotateMinus = yRotateMinus;
        _cancel = cancel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static KeyCode GetKey(byte index)
    {
        switch (index)
        {
            case 0: return _xRotatePlus;
            case 1: return _xRotateMinus;
            case 2: return _yRotatePlus;
            case 3: return _yRotateMinus;
            case 4: return _cancel;
        }
        return _yRotateMinus;
    }

    public static Material GetMaterial(bool deployable)
    {
        if (deployable)
        {
            return _deployable;
        }
        else
        {
            return _notDeployable;
        }
    }

    public static GameObject GetPointer()
    {
        return _pointerPrefab;
    }
}
