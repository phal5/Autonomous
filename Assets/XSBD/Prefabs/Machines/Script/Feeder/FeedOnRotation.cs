using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedOnRotation : MonoBehaviour
{
    [SerializeField] Transform _rotatingPart;
    [SerializeField] Feeder _feeder;
    [SerializeField] float _angularThreshold;
    [Space(10f)]
    [SerializeField] bool _resetWhenStopped;

    float _resetTime = 1;
    float _angle = 0;
    float _timer = 0;
    bool _stoppedAndFed = false;
    bool _feed;

    // Update is called once per frame
    void Update()
    {
        UpdateAnglesAndFeed();
        Feed();
    }

    float Abs(float f)
    {
        return (f > 0) ? f : -f;
    }

    float Floor360(float f)
    {
        return (f > 360) ? f - 360 : f;
    }

    void UpdateAnglesAndFeed()
    {
        if(_angle != _rotatingPart.transform.eulerAngles.y)
        {
            _timer = 0;
            _stoppedAndFed = false;
            if (Floor360(Abs(_angle - _rotatingPart.transform.eulerAngles.y)) > _angularThreshold)
            {
                _angle = _rotatingPart.transform.eulerAngles.y;
                _feed = true;
            }
        }
        else if(_resetWhenStopped)
        {
            Tick();
        }
    }

    void Tick()
    {
        if (!_stoppedAndFed)
        {
            _timer += Time.deltaTime;
            if(_timer > _resetTime)
            {
                _timer = 0;
                _angle = _rotatingPart.transform.eulerAngles.y;
                _stoppedAndFed = true;
                _feed = true;
            }
        }
    }

    void Feed()
    {
        if (_feed && _feeder.Feed())
        {
            _feed = false;
        }
    }
}
