using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AdjustRopeLength : MonoBehaviour
{
    [SerializeField] bool lookUp = true;
    [SerializeField] Transform _target;
    float _scaleDivisor;
    // Start is called before the first frame update
    void Start()
    {
        _scaleDivisor = transform.localScale.y / transform.lossyScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Stretch();
    }

    void Stretch()
    {
        if (lookUp)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            transform.LookAt(_target.position);
            transform.rotation *= Quaternion.Euler(Vector3.right * 90);
        }
        transform.localScale = Vector3.Dot(transform.up, (_target.position - transform.position)) * _scaleDivisor * Vector3.up
            + Vector3.Scale(Vector3.forward + Vector3.right, transform.localScale);
    }
}
