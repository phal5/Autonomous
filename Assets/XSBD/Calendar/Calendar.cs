using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Calendar : MonoBehaviour
{
    [System.Serializable] class ToDo
    {
        [SerializeField] UnityEvent _toDo = new UnityEvent();
        [SerializeField] byte[] _onTheseDays;

        public void Invoke(byte date)
        {
            foreach (byte day in _onTheseDays)
            {
                if (day == date)
                {
                    _toDo.Invoke();
                    break;
                }
            }
        }
    }

    [SerializeField] UnityEvent _toDoEveryDay = new UnityEvent();
    [SerializeField] ToDo[] _toDoList;
    [Space(10f)]
    [Tooltip("Length of day in seconds.")] [SerializeField] float dayNightLength;
    [SerializeField] List<byte> _lengthPerMonths = new List<byte>();

    static Light _mainLight;
    static float _dayNightDivisor;
    static float _timer = 0.375f;
    static byte day = 1;
    static byte month = 1;
    
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
        UpdateSun();
        SetDay();
    }

    void DayLengthUpdater()
    {
        float dayNightOne = _dayNightDivisor * dayNightLength;
        if (dayNightOne < 0.999f || dayNightOne > 1.001f)
        {
            _dayNightDivisor = 1 / dayNightLength;
        }
    }

    void UpdateSun()
    {
        transform.eulerAngles = new Vector3(360f * _timer - 90, -90, 0);
        _mainLight.intensity = Vector3.Dot(-transform.forward, Vector3.up);
    }

    void SetDay()
    {
        _timer += Time.deltaTime * _dayNightDivisor;
        if (_timer >= 1)
        {
            _timer = 0;
            ++day;
            CheckToDoList();

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

    void CheckToDoList()
    {
        _toDoEveryDay.Invoke();
        foreach (ToDo toDo in _toDoList)
        {
            toDo.Invoke(day);
        }
    }

    /* public float GetTimer()
    {
        return _timer;
    }
    */

    public static byte GetDay()
    {
        return day;
    }

    public static byte GetMonth()
    {
        return month;
    }

    public static float GetTimer()
    {
        return _timer;
    }

    public static bool IsDaytime()
    {
        return _timer > 0.25f && _timer < 0.75f;
    }
}
