using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoat : MonoBehaviour
{
    [SerializeField] Transform _rayOriginPos;
       
    RaycastHit hit;

    GameObject _player;
    Rigidbody _playerRB;
    float _playerPosY;
    void Start()
    {
        _player = PlayerManager.GetPlayer();
        _playerRB = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (OnWater()) MakePlayerFloat();
        else _playerRB.constraints = RigidbodyConstraints.FreezeRotation;
    }

    bool OnWater()
    {
        Physics.Raycast(_rayOriginPos.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity);

        if (hit.transform.gameObject != null && hit.transform.gameObject.layer == 4)
        {
            return true; // water ·¹ÀÌ¾î 
        }
        return false;
    }

    void MakePlayerFloat()
    {
        //_player.transform.position = new Vector3(_player.transform.position.x, _playerPosY, _player.transform.position.z);
        _playerRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }


    void OnTriggerEnter(Collider other)
    {

    }

    void OnTriggerStay(Collider other)
    {
     

    }
    void OnTriggerExit(Collider other)
    {
        
    }
}
