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
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            RollerRotate();
            AddRollerTag();
        }
    }

    void TimerAccumulate()
    {
        _timer += _Speed();
    }

    void RollerRotate()
    {
        transform.eulerAngles += new Vector3(1,0,0) * _Speed() * Time.deltaTime;
    }

    void AddRollerTag()
    {
        gameObject.tag = "Kneader";
    }

}
