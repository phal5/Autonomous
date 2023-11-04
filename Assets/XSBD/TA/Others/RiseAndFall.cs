using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseAndFall : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _height;

    float _timer;
    float _initialY;

    // Start is called before the first frame update
    void Start()
    {
        _initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime * _speed;
        if(_timer > Mathf.PI * 2)
        {
            _timer -= Mathf.PI * 2;
        }
        transform.position =
            Vector3.Scale(transform.position, Vector3.right + Vector3.forward)
            + _initialY * Vector3.up
            + (Mathf.Sin(_timer) + 1) * _height * 0.5f * Vector3.up;
    }
}
