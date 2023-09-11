using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : Power
{
    [SerializeField] Transform _seeSaw;
    enum Axis { X, Y, Z };
    Axis _axis;
    float _angle;
    // Start is called before the first frame update
    void Start()
    {
        if (!_seeSaw)
        {
            _seeSaw = transform;
        }
        SetRotation();
    }

    // Update is called once per frame
    void Update()
    {
        GetRotationDelta();
    }

    void GetRotationDelta()
    {
        switch (_axis)
        {
            case Axis.X:
                _Speed = Abs(_seeSaw.rotation.x - _angle);
                _angle = _seeSaw.rotation.x;
                break;
            
            case Axis.Y:
                _Speed = Abs(_seeSaw.rotation.y - _angle);
                _angle = _seeSaw.rotation.y;
                break;
            
            case Axis.Z:
                _Speed = Abs(_seeSaw.rotation.z - _angle);
                _angle = _seeSaw.rotation.z;
                break;
        }
    }

    void SetRotation()
    {
        switch (_axis)
        {
            case Axis.X:
                _angle = _seeSaw.rotation.x;
                break;

            case Axis.Y:
                _angle = _seeSaw.rotation.y;
                break;

            case Axis.Z:
                _angle = _seeSaw.rotation.z;
                break;
        }
    }

    float Abs(float number)
    {
        return (number > 0)? number : -number;
    }
}
