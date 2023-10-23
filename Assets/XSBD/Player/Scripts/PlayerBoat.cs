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
        _playerRB = PlayerManager.GetRigidbody();

    }

    void Update()
    {
        if (OnWater()) MakePlayerFloat();
    }

    bool OnWater()
    {
        Physics.Raycast(_rayOriginPos.position, transform.TransformDirection(Vector3.down), out hit, 10f);

        if (hit.transform.gameObject.layer == 4) return true; // water ·¹ÀÌ¾î 
        else return false;
    }

    void MakePlayerFloat()
    {
        _playerPosY = _player.transform.position.y;
        _player.transform.position = new Vector3(_player.transform.position.x, _playerPosY, _player.transform.position.z);
    }
}
