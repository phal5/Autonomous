using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable] public class SpeechCard
{
    [System.Serializable] class ImageData
    {
        [SerializeField] GameObject _imagePrefab;
        [SerializeField] Xpos _xPosition = Xpos.MIDDLE;
        [SerializeField] Ypos _yPosition = Ypos.BOTTOM;

        public GameObject GetImage()
        {
            return _imagePrefab;
        }
        public Xpos GetX()
        {
            return _xPosition;
        }
        public Ypos GetY()
        {
            return _yPosition;
        }
    }

    enum Ypos { TOP, MIDDLE, BOTTOM }
    enum Xpos { LEFT, MIDDLE, RIGHT }

    [SerializeField] string _speaker;
    [SerializeField] string _speech;
    [SerializeField] ImageData _imageData;

    static GameObject _image;

    public void PrintSpeech(TextMeshProUGUI speaker, TextMeshProUGUI speech)
    {
        speaker.text = _speaker;
        speech.text = _speech;

        if(_image != null)
        {
            MonoBehaviour.Destroy(_image.gameObject);
        }
        if(_imageData.GetImage() != null)
        {
            _image = MonoBehaviour.Instantiate(_imageData.GetImage(), SpeechManager.Data.ImageParent().transform);
            if (_image.TryGetComponent<RectTransform>(out RectTransform image))
            {
                image.anchoredPosition = SetPosition(image);
            }
            else
            {
                MonoBehaviour.Destroy(_image.gameObject);
            }
        }
    }

    public static void ClearImage()
    {
        MonoBehaviour.Destroy(_image.gameObject);
    }

    Vector2 SetPosition(RectTransform image)
    {
        Vector2 position = Vector2.zero;
        switch (_imageData.GetX())
        {
            case Xpos.LEFT: position.x = - SpeechManager.Data.ImageArea().rect.width + image.rect.width; break;
            case Xpos.RIGHT: position.x = SpeechManager.Data.ImageArea().rect.width - image.rect.width; break;
            default: break;
        }
        switch (_imageData.GetY())
        {
            case Ypos.TOP: position.y = SpeechManager.Data.ImageArea().rect.height - image.rect.height; break;
            case Ypos.BOTTOM: position.y = - SpeechManager.Data.ImageArea().rect.height + image.rect.height; break;
            default: break;
        }
        return position * 0.5f + SpeechManager.Data.ImageArea().anchoredPosition;
    }
}
