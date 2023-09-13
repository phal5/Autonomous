using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HenSeeSawPower : Rotation
{
    [System.Serializable] class Feeders
    {
        [SerializeField] Feeder _feeder;
        [Tooltip("Between -180 and 180")] [SerializeField] float _threshold;
        [SerializeField] bool _set = true;

        public void Feed(float angle, bool greaterLesser)
        {
            angle = (angle < 180) ? angle : angle - 360;
            if (_set && greaterLesser == angle > _threshold)
            {
                _set = false;
                Debug.Log("Yup");
                _feeder.Feed();
            }
            else if(!_set && greaterLesser == angle < _threshold)
            {
                _set = true;
            }
        }
    }
    [SerializeField] Feeders[] _feeders = new Feeders[2];

    // Start is called before the first frame update
    void Start()
    {
        if (!_seeSaw)
        {
            _seeSaw = transform;
        }
        SetRotation();
    }

    // Update is called once per frame
    void Update()
    {
        GetRotationDelta();
        _feeders[0].Feed(transform.eulerAngles.x, true);
        _feeders[1].Feed(transform.eulerAngles.x, false);
    }
}
