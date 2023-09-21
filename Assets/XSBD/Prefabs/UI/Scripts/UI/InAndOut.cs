using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAndOut : MonoBehaviour
{
    enum Direction { UP, DOWN, LEFT, RIGHT }
    [SerializeField] KeyCode _key;
    [SerializeField] Direction _direction;
    [SerializeField] float _speed = 10;

    RectTransform _UI;
    Vector3 _dir;
    Vector3 _initial;
    Vector3 _hidden;
    float _timer;
    bool _show;

    void Start()
    {
        if(!TryGetComponent<RectTransform>(out _UI))
        {
            Destroy(this);
        }

        _initial = _UI.transform.position;

        switch (_direction)
        {
            case Direction.UP: _dir = Vector3.up * _UI.rect.height * 0.1f; break;
            case Direction.DOWN: _dir = Vector3.down * _UI.rect.height * 0.1f; break;
            case Direction.RIGHT: _dir = Vector3.right * _UI.rect.width * 0.1f; break;
            case Direction.LEFT: _dir = Vector3.left * _UI.rect.width * 0.1f; break;
        }
        _hidden = _UI.transform.position + _dir;
        _timer = 1;
        transform.position = _hidden;
    }

    // Update is called once per frame
    void Update()
    {
        ShowAndHide();
        Move();
    }

    void ShowAndHide()
    {
        if (Input.GetKeyDown(_key))
        {
            if (_show)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    public void Show()
    {
        _show = true;
        if(transform.parent.TryGetComponent<InAndOut>(out InAndOut io))
        {
            io.Show();
        }
        
    }

    public void Hide()
    {
        _show = false;
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent<InAndOut>(out InAndOut io))
            {
                io.Hide();
            }
        }
    }

    public void Move()
    {
        if (_show)
        {
            if(_timer > 0)
            {
                _timer -= Time.deltaTime * _speed;
                if (_timer < 0)
                {
                    _timer = 0;
                }
                float timer = 0.5f - Mathf.Cos(_timer * Mathf.PI) * 0.5f;
                transform.position = Vector3.Lerp(_initial, _hidden, timer);
            }
        }
        else
        {
            if(_timer < 1)
            {
                _timer += Time.deltaTime * _speed;
                if (_timer > 1)
                {
                    _timer = 1;
                }
                float timer = 0.5f - Mathf.Cos(_timer * Mathf.PI) * 0.5f;
                transform.position = Vector3.Lerp(_initial, _hidden, timer);
            }
        }
    }
}
