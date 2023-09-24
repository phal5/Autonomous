using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovementManager : MonoBehaviour
{
    static PlayerMove _playerMove;
    // Start is called before the first frame update
    void Awake()
    {
        transform.TryGetComponent<PlayerMove>(out _playerMove);
    }

    public static void Enable(bool enability)
    {
        if(_playerMove != null)
        {
            _playerMove.enabled = enability;
        }
    }
}
