using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInertiaTensor : MonoBehaviour
{
    Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.inertiaTensorRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
