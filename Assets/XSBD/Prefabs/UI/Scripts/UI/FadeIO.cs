using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIO : MonoBehaviour
{
    [SerializeField] float _speed = 3;
    [SerializeField] KeyCode _key;
    [SerializeField] bool _in = false;

    MaskableGraphic _image;
    float _fadeTime;

    void Start()
    {
        _image = GetComponent<MaskableGraphic>();
        _image.CrossFadeAlpha((_in == true) ? 1 : 0, 0, true);
        _image.raycastTarget = _in;
        _fadeTime = 1 / _speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            _image.CrossFadeAlpha((_in ^= true) ? 1 : 0, _fadeTime, true);
            _image.raycastTarget = _in;
        }
    }
}
