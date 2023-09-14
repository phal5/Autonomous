using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSaw : Power
{
    [SerializeField] Rigidbody _rigidbody1;
    [SerializeField] Rigidbody _rigidbody2;
    [SerializeField] float _friction = 0;
    float _1;
    float _2;
    // Start is called before the first frame update
    void Start()
    {
        _1 = _rigidbody1.transform.position.y;
        _2 = _rigidbody2.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        DecidePosition(out _Speed);
        DecideVelocity();
        SavePosition();
    }

    void DecidePosition(out float speed)
    {
        float shift1 = (_rigidbody1.transform.position.y - _1);
        float shift2 = (_rigidbody2.transform.position.y - _2);
        float negated = shift1 - shift2;
        negated *= 0.5f;
        speed = negated;
        negated = (negated > 0) ? negated : -negated;

        _rigidbody1.transform.position = _1 * Vector3.up + Vector3.Scale(Vector3.forward + Vector3.right, _rigidbody1.transform.position);
        _rigidbody2.transform.position = _2 * Vector3.up + Vector3.Scale(Vector3.forward + Vector3.right, _rigidbody2.transform.position);

        if (shift1 > shift2)
        {
            _rigidbody1.transform.position += Vector3.up * negated;
            _rigidbody2.transform.position -= Vector3.up * negated;
        }
        else
        {
            _rigidbody1.transform.position -= Vector3.up * negated;
            _rigidbody2.transform.position += Vector3.up * negated;
        }
    }

    void DecideVelocity()
    {
        float negated = _rigidbody1.velocity.y - _rigidbody2.velocity.y;
        negated *= 0.5f - Time.deltaTime * _friction;
        negated = (negated > 0) ? negated : -negated;
        if (_rigidbody1.velocity.y > _rigidbody2.velocity.y)
        {
            _rigidbody1.velocity = negated * Vector3.up + ReturnHorizontal(_rigidbody1.velocity);
            _rigidbody2.velocity = -negated * Vector3.up + ReturnHorizontal(_rigidbody1.velocity);
        }
        else
        {
            _rigidbody2.velocity = negated * Vector3.up + ReturnHorizontal(_rigidbody1.velocity);
            _rigidbody1.velocity = -negated * Vector3.up + ReturnHorizontal(_rigidbody1.velocity);
        }
    }

    void SavePosition()
    {
        _1 = _rigidbody1.transform.position.y;
        _2 = _rigidbody2.transform.position.y;
    }

    Vector3 ReturnHorizontal(Vector3 vector3)
    {
        return Vector3.Scale(vector3, Vector3.forward + Vector3.right);
    }
}
