using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBoat : MonoBehaviour
{
    [SerializeField] Transform _rayOriginPos;
    [SerializeField] UnityEvent _enterEvent;
    [SerializeField] UnityEvent _exitEvent;
    [SerializeField] float _depth = 0.5f;

    Rigidbody _playerRB;
    RaycastHit hit;
    int layerMask = 1 << 4;
    bool _onWater;

    void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (OnWater())
        {
            MakePlayerFloat();
            if (_onWater == false)
            {
                _onWater = true;
                _enterEvent.Invoke();
            }
        }
        else
        {
            if (_onWater == true)
            {
                _onWater = false;
                _exitEvent.Invoke();
            }
        }
    }

    bool OnWater()
    {
        return Physics.Raycast(_rayOriginPos.position + Vector3.up * 2, transform.TransformDirection(Vector3.down), out hit, _rayOriginPos.localPosition.y + 2 - _depth, layerMask);
    }

    void MakePlayerFloat()
    {
        float seaPosY = hit.transform.position.y + hit.transform.localScale.y / 2;
        _playerRB.velocity = Vector3.Scale(_playerRB.velocity, Vector3.forward + Vector3.right);

        transform.position = new Vector3(transform.position.x, seaPosY - _depth, transform.position.z);
    }

}
