using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineDeployer : Deployer
{
    [SerializeField] GameObject _pointer;
    [SerializeField] bool _xRotatable;
    [SerializeField] bool _liftable;
    [SerializeField] KeyCode _xRotatePlus = KeyCode.I;
    [SerializeField] KeyCode _xRotateMinus = KeyCode.K;
    [SerializeField] KeyCode _yRotatePlus = KeyCode.J;
    [SerializeField] KeyCode _yRotateMinus = KeyCode.L;

    RaycastHit hit;
    float _heightOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 2;
        
        PlayerMovementManager.Enable(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetHeight();
        SetRotation();
        SetPosition();
        Place();
    }

    void SetPosition()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            transform.position = hit.point + Vector3.up * _heightOffset;
        }
    }

    void SetHeight()
    {
        if (_liftable)
        {
            _heightOffset -= Input.mouseScrollDelta.y;
        }
    }

    void SetRotation()
    {
        if (_xRotatable)
        {
            Rotate(_xRotatePlus, Vector3.right * 5);
            Rotate(_xRotateMinus, Vector3.right * -5);
        }
        Rotate(_yRotatePlus, Vector3.up * 5);
        Rotate(_yRotateMinus, Vector3.up * -5);
    }

    void Rotate(KeyCode key, Vector3 rotation)
    {
        if (Input.GetKeyDown(key))
        {
            transform.eulerAngles += rotation;
        }
    }

    void Place()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Deploy(transform.position, transform.rotation))
            {
                PlayerMovementManager.Enable(true);
            }
        }
    }
}
