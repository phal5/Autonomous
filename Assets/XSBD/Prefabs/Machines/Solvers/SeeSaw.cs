using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSaw : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody1;
    [SerializeField] Rigidbody _rigidbody2;
    [SerializeField] float _friction = 0;
    Vector3 _1;
    Vector3 _2;
    // Start is called before the first frame update
    void Start()
    {
        _1 = _rigidbody1.transform.position;
        _2 = _rigidbody2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float shift1 = (_rigidbody1.transform.position.y - _1.y);
        float shift2 = (_rigidbody2.transform.position.y - _2.y);
        float negated = shift1 - shift2;
        negated *= 0.5f;
        negated = (negated > 0) ? negated : -negated;
        if(shift1 > shift2)
        {
            _rigidbody1.transform.position = _1;
            _rigidbody2.transform.position = _2;

            _rigidbody1.transform.position += Vector3.up * negated;
            _rigidbody2.transform.position -= Vector3.up * negated;
        }
        else
        {
            _rigidbody1.transform.position = _1;
            _rigidbody2.transform.position = _2;

            _rigidbody1.transform.position -= Vector3.up * negated;
            _rigidbody2.transform.position += Vector3.up * negated;
        }

        negated = _rigidbody1.velocity.y - _rigidbody2.velocity.y;
        negated *= 0.5f - Time.deltaTime * _friction;
        negated = (negated > 0)? negated : -negated;
        if(_rigidbody1.velocity.y > _rigidbody2.velocity.y)
        {
            _rigidbody1.velocity = negated * Vector3.up + ReturnHorizontal(_rigidbody1.velocity);
            _rigidbody2.velocity = - negated * Vector3.up + ReturnHorizontal(_rigidbody1.velocity);
        }
        else
        {
            _rigidbody2.velocity = negated * Vector3.up + ReturnHorizontal(_rigidbody1.velocity);
            _rigidbody1.velocity = - negated * Vector3.up + ReturnHorizontal(_rigidbody1.velocity);
        }


        _1 = _rigidbody1.transform.position;
        _2 = _rigidbody2.transform.position;
    }

    Vector3 ReturnHorizontal(Vector3 vector3)
    {
        return Vector3.Scale(vector3, Vector3.forward + Vector3.right);
    }
}
