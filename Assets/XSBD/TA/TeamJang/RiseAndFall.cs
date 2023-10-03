using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseAndFall : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _height;

    float _timer;
    Vector3 _initial;

    // Start is called before the first frame update
    void Start()
    {
        _initial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime * _speed;
        if(_timer > Mathf.PI * 2)
        {
            _timer -= Mathf.PI * 2;
        }
        transform.position = _initial + (Mathf.Sin(_timer) + 1) * _height * 0.5f * Vector3.up;
    }
}
