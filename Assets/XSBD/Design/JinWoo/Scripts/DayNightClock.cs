using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightClock : MonoBehaviour
{
    public int dayNightLength; //Length of a game day in seconds.
    public bool doDayNightCycle; //whether or not if the dayNightCycle runs.
    public byte dayNightTime = 72; //In-game time. The unit is 10 minutes. 0 equals midnight. (72 equals noon)
    public byte dayCount = 1; //The in-game day.
    public byte monthCount = 1; //The in-game month. Actual month rules apply. (Except for leap years!)
    public List<byte> _31dayMonths = new List<byte>();
    public List<byte> _30dayMonths = new List<byte>();
    private float tenMinuteTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        //rotate the sun to noon
        transform.eulerAngles = new Vector3 (34, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //clock module. Adds 10 minutes to the dayNightTime whenever the specified time has passed.
        if (doDayNightCycle)
        {
            tenMinuteTimer += Time.deltaTime;
            transform.Rotate(new Vector3 (0, 360f*Time.deltaTime / dayNightLength, 0), Space.Self);
            if (tenMinuteTimer >= dayNightLength/144f)
            {
                tenMinuteTimer = 0;
                dayNightTime++; 
            }
            if(dayNightTime >=  144)
            {
                dayNightTime = 0;
                dayCount++;
                //rotate the sun to sync it with dayNightTime
                transform.eulerAngles = new Vector3(-34, -180, 0);
            }
            //months.
            if(dayCount == 29 && monthCount == 2)
            {
                dayCount = 1;
                monthCount++;
            }
            if (dayCount == 31)
            {
                foreach (byte b in _30dayMonths)
                {
                    if (monthCount == b)
                    {
                        dayCount = 1;
                        monthCount++;
                        break;
                    }
                }
            }
            if (dayCount == 32)
            {
                foreach (byte b in _31dayMonths)
                {
                    if (monthCount == b)
                    {
                        dayCount = 1;
                        monthCount++;
                        break;
                    }
                }
            }
            if(monthCount > 12)
            {
                monthCount = 1;
            }
        }
    }
}
