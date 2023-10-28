using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MatchPosition : MonoBehaviour
{
    [Tooltip("The Script Relies On Renderers!")]
    [SerializeField] GameObject _1;
    [SerializeField] GameObject _2;
    [SerializeField] bool _move_2_to_1;
    Renderer _renderer1;
    Renderer _renderer2;

    // Update is called once per frame
    void Update()
    {
        if (_move_2_to_1)
        {
            _move_2_to_1 = false;
            SetPosition();
        }
    }

    void SetPosition()
    {
        _renderer1 = _1.GetComponent<Renderer>();
        _renderer2 = _2.GetComponent<Renderer>();
        Vector3 _pos1 = _renderer1.bounds.center;
        Vector3 _pos2 = _renderer2.bounds.center;
        _2.transform.position += _pos1 - _pos2;
    }
}
