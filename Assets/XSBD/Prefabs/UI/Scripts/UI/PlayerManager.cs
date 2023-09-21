using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject _Player;
    [SerializeField] GameObject _Camera;
    [SerializeField] Vector3 _EyesOffset;
    static GameObject _player;
    static GameObject _camera;
    static Vector3 _eyesOffset;
    // Start is called before the first frame update
    void Start()
    {
        if(_Player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            _player = _Player;
        }
        if (_Camera == null)
        {
            _camera = Camera.main.gameObject;
        }
        else
        {
            _camera = _Camera;
        }
        _eyesOffset = _EyesOffset;
    }

    public static GameObject GetPlayer()
    {
        return _player;
    }

    public static Vector3 GetCamPosition()
    {
        return _camera.transform.position;
    }
}
