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
    static Rigidbody _rigidbody;
    static PlayerWalk _playerWalk;
    static Vector3 _eyesOffset;

    // Start is called before the first frame update
    void Start()
    {
        if(_Player == null) _player = GameObject.FindGameObjectWithTag("Player");
        else _player = _Player;
        if(!_player.TryGetComponent<Rigidbody>(out _rigidbody)) Debug.LogError("No Rigidbody found in player!");
        if (!_player.TryGetComponent<PlayerWalk>(out _playerWalk)) Debug.LogError("No PlayerWalk found in player!");
        if (_Camera == null) _camera = Camera.main.gameObject;
        else _camera = _Camera;
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

    public static Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public static void SetWalkability(bool enable)
    {
        _playerWalk.enabled = enable;
    }
}
