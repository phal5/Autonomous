using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabbageGrid : Action
{
    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Collider>(out _collider);
    }

    // Update is called once per frame
    void Update()
    {
        Act();
    }

    void Act()
    {
        if(_Speed() != 0)
        {
            transform.rotation *= Quaternion.Euler(Vector3.right * _Speed());
            _collider.enabled = _Speed() < 10 * Time.deltaTime;
        }
    }

    void SetXAngle(float xAngle)
    {
        transform.localEulerAngles = Vector3.right * xAngle + Vector3.up * 180;
    }
}
