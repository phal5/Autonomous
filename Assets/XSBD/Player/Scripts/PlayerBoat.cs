using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoat : MonoBehaviour
{
    [SerializeField] Transform _rayOriginPos;
    [SerializeField] PlayerManager _playerManager;
       
    RaycastHit hit;

    void Update()
    {
        if (OnWater()) MakePlayerFloat();
    }

    bool OnWater()
    {
        Physics.Raycast(_rayOriginPos.position, transform.TransformDirection(Vector3.down), out hit, 10f);

        if (hit.transform.gameObject.layer == 4) return true;
        else return false;
    }

    void MakePlayerFloat()
    {
        Rigidbody playerRB = _playerManager.GetRigidBody();
    }
}
