using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBar : MonoBehaviour
{
    Image _image;
    Scrollbar _scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _scrollbar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        _image.fillAmount = 1 - _scrollbar.value;
    }
}
