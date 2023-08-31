using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightClock : MonoBehaviour
{
    [Tooltip("Length of day in seconds.")] [SerializeField] float dayNightLength;
    [SerializeField] byte day = 1;
    [SerializeField] byte month = 1;
    [SerializeField] List<byte> _lengthPerMonths = new List<byte>();
    Light _mainLight;
    float _dayNightDivisor;
    float _timer = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _mainLight = GetComponent<Light>();
        _dayNightDivisor = 1 / dayNightLength;
    }

    // Update is called once per frame
    void Update()
    {
        DayLengthUpdater();
        Timer();
        UpdateSun();
        Calendar();
    }

    void DayLengthUpdater()
    {
        float dayNightOne = _dayNightDivisor * dayNightLength;
        if (dayNightOne < 0.999f || dayNightOne > 1.001f)
        {
            _dayNightDivisor = 1 / dayNightLength;
            Debug.Log("Changed");
        }
    }

    void Timer()
    {
        _timer += Time.deltaTime * _dayNightDivisor;
    }

    void UpdateSun()
    {
        transform.eulerAngles = new Vector3(360f * _timer - 90, -90, 0);
        _mainLight.intensity = Vector3.Dot(-transform.forward, Vector3.up);
    }

    void Calendar()
    {
        if (_timer >= 1)
        {
            _timer = 0;
            ++day;
            if (day > _lengthPerMonths[month])
            {
                ++month;
                day = 1;
                if (month >= _lengthPerMonths.Count)
                {
                    month = 1;
                }
            }
        }
    }

    public float GetTimer()
    {
        return _timer;
    }

    public byte GetDay()
    {
        return day;
    }

    public byte GetMonth()
    {
        return month;
    }
}
