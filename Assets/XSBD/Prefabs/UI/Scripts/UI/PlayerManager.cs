using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject _Player;
    [SerializeField] GameObject _Camera;
    [SerializeField] InventoryInstance _Inventory;
    [SerializeField] Vector3 _EyesOffset;

    static GameObject _player;
    static GameObject _camera;
    static InventoryInstance _inventory;
    static Rigidbody _rigidbody;
    static PlayerMove _playerMove;
    static PlayerWalk _playerWalk;
    static Vector3 _eyesOffset;

    // Start is called before the first frame update
    void Start()
    {
        if(_Player == null) _player = GameObject.FindGameObjectWithTag("Player");
        else _player = _Player;

        if (!_player.TryGetComponent<Rigidbody>(out _rigidbody)) Debug.LogError("No Rigidbody found in player!");
        if (!SearchComponentInChildren<PlayerWalk>(_player.transform, out _playerWalk)) Debug.LogError("No PlayerWalk found in player!");
        if (!SearchComponentInChildren<PlayerMove>(_player.transform, out _playerMove)) Debug.LogError("No PlayerMove found in player!");

        if (_Camera == null) _camera = Camera.main.gameObject;
        else _camera = _Camera;
        _eyesOffset = _EyesOffset;

        if (_Inventory == null) GameObject.Find("PlayerInventory").TryGetComponent<InventoryInstance>(out _inventory);
        else _inventory = _Inventory;
    }

    bool SearchComponentInChildren<T>(Transform transform, out T component)
    {
        if(transform.TryGetComponent<T>(out component)) return true;
        foreach (Transform child in transform)
        {
            if(SearchComponentInChildren<T>(child, out component)) return true;
        }
        return false;
    }

    //---

    public static GameObject GetPlayer()
    {
        return _player;
    }

    public static Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public static InventoryInstance GetInventoryInstance()
    {
        return _inventory;
    }

    public static PlayerWalk GetPlayerWalkAnim()
    {
        return _playerWalk;
    }

    public static PlayerMove GetPlayerMove()
    {
        return _playerMove;
    }

    public static Vector3 GetCamPosition()
    {
        return _camera.transform.position;
    }

    //---

    public static void SetWalkability(bool enable)
    {
        _playerMove.enabled = enable;
    }
}
