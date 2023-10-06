using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAndAttatch : MonoBehaviour
{
    [SerializeField] GameObject _object;
    [SerializeField] Transform _parent;
    [SerializeField] float _time;
    GameObject _instance;
    float _timer;

    private void Start()
    {
        _timer = _time;
    }

    private void OnEnable()
    {
        _timer = _time;
    }

    // Update is called once per frame
    void Update()
    {
        if(_instance == null)
        {
            _timer -= Time.deltaTime;
            if( _timer < 0 ) _instance = Instantiate(_object, _parent);
        }
    }

    private void OnDisable()
    {
        Destroy(_instance);
    }
}
