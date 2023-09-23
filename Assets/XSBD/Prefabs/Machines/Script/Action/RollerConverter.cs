using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerConverter : Action
{
    float _timer;

    // Update is called once per frame
    void Update()
    {
        TimerAccumulate();
        if (_timer > 0.1f)
        {
            _timer -= Time.deltaTime;
            RollerRotate();
            AddRollerTag();
        }
        else
        {
            gameObject.tag = "Untagged";
        }
    }

    void TimerAccumulate()
    {
        _timer += (_Speed() > 0)? _Speed() : -_Speed();
    }

    void RollerRotate()
    {
        transform.rotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * 100);
    }

    void AddRollerTag()
    {
        gameObject.tag = "Kneader";
    }

}
