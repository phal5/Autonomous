using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAndOut : MonoBehaviour
{
    enum Direction { UP, DOWN, LEFT, RIGHT }
    [SerializeField] KeyCode _key;
    [SerializeField] Direction _direction;
    [SerializeField] float _speed = 10;

    Canvas _canvas;
    RectTransform _UI;
    Vector3 _initial;
    Vector3 _hidden;
    Vector2 _dir;
    float _timer;
    bool _show;

    void Start()
    {
        if( _canvas == null)
        {

        }

        if(!TryGetComponent<RectTransform>(out _UI))
        {
            Destroy(this);
        }

        _initial = _UI.transform.position;
        
        switch (_direction)
        {
            case Direction.UP: _dir = Vector2.up * (_UI.rect.height * 0.5f + Screen.height * 0.5f - _UI.anchoredPosition.y); break;
            case Direction.DOWN: _dir = Vector2.down * (_UI.rect.height * 0.5f + Screen.height * 0.5f + _UI.anchoredPosition.y); break;
            case Direction.RIGHT: _dir = Vector2.right * (_UI.rect.width * 0.5f + Screen.width * 0.5f - _UI.anchoredPosition.x); break;
            case Direction.LEFT: _dir = Vector2.left * (_UI.rect.width * 0.5f + Screen.width * 0.5f + _UI.anchoredPosition.x); break;
            default: _dir = Vector2.zero; break;
        }
        _UI.anchoredPosition += _dir;
        _timer = 1;
        _hidden = _UI.position;
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
