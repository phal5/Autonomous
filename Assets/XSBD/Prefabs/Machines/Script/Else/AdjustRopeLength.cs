using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustRopeLength : MonoBehaviour
{
    [SerializeField] Transform _transform;
    [SerializeField] bool lookUp;
    float _scaleDivisor;
    // Start is called before the first frame update
    void Start()
    {
        _scaleDivisor = transform.localScale.y / transform.lossyScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.up * (_transform.position.y - transform.position.y) * _scaleDivisor
            + Vector3.Scale(Vector3.forward + Vector3.right, transform.localScale);
        if (lookUp)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
