using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineDeployer : Deployer
{
    [SerializeField] KeyCode _xRotatePlus = KeyCode.W;
    [SerializeField] KeyCode _xRotateMinus = KeyCode.S;
    [SerializeField] KeyCode _yRotatePlus = KeyCode.A;
    [SerializeField] KeyCode _yRotateMinus = KeyCode.D;
    [SerializeField] bool _xRotatable;
    RaycastHit hit;
    float _heightOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
        Place();
    }

    void Place()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            transform.position = hit.point + Vector3.up * _heightOffset;
        }
    }

    void Scroll()
    {
        Rotate(_xRotatePlus, Vector3.right * 5);
        Rotate(_xRotateMinus, Vector3.right * -5);
        Rotate(_yRotatePlus, Vector3.up * 5);
        Rotate(_yRotateMinus, Vector3.up * -5);
        _heightOffset -= Input.mouseScrollDelta.y;
    }

    void Rotate(KeyCode key, Vector3 rotation)
    {
        if (Input.GetKeyDown(key))
        {
            transform.eulerAngles += rotation;
        }
    }
}
