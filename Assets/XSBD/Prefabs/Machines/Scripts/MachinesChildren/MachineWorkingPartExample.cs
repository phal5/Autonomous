using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineWorkingPartExample : MonoBehaviour
{
    float _speedAccum;
    Machines _machine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _speedAccum += _machine.GetSpeed();

        
    }


}
