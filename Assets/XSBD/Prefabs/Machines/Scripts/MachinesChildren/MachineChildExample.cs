using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineChildExample : Machines
{
    //Machines
    //protected float _speed

    float _rotY;
    // Start is called before the first frame update
    void Start()
    {
        _rotY = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        _speed = GetDeltaAngle();
        SavePriorAngle();
    }

    float GetDeltaAngle()
    {
        return _rotY - transform.eulerAngles.y;
    }

    void SavePriorAngle()
    {
        _rotY = transform.eulerAngles.y;
    }
}
