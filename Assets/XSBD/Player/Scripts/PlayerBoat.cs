using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBoat : MonoBehaviour
{
    [SerializeField] Transform _rayOriginPos;
    [SerializeField] UnityEvent _enterEvent;
    [SerializeField] UnityEvent _exitEvent;
    
    RaycastHit hit;

    Rigidbody _playerRB;
    [SerializeField] float _depth = 0.5f;

    int layerMask = 1 << 4;

    void Start()
    {
        
        _playerRB = GetComponent<Rigidbody>();
        //_enterEvent.Invoke();
    }

    void Update()
    {
        print(OnWater());
        if (OnWater()) MakePlayerFloat();
      

    }

    bool OnWater()
    {
        return Physics.Raycast(_rayOriginPos.position + Vector3.up*2, transform.TransformDirection(Vector3.down), out hit, _rayOriginPos.localPosition.y + 2 - _depth, layerMask);
    }

    void MakePlayerFloat()
    {
        float seaPosY = hit.transform.position.y + hit.transform.localScale.y / 2;
        _playerRB.velocity = Vector3.Scale(_playerRB.velocity, Vector3.forward + Vector3.right);

        transform.position = new Vector3(transform.position.x, seaPosY-_depth ,transform.position.z);

        //_playerRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

}
