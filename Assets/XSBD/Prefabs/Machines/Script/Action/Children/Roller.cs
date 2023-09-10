using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : Action
{
    [Space(10f)]
    [SerializeField] bool _absoluteAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, (_absoluteAngle)? (_Speed() > 0) ? _Speed() : -_Speed() : _Speed(), 0);
    }
}
