using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] float _randomness;
    [SerializeField] GameObject _growInto;

    void Start()
    {
        _timer += (Random.value - 0.5f) * _randomness * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
        else
        {
            Instantiate(_growInto, transform.position, transform.rotation, null);
            Destroy(gameObject);
        }
    }
}
