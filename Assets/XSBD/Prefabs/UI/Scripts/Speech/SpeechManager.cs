using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    [System.Serializable] public class Data
    {
        [SerializeField] TextMeshProUGUI _speakerBox;
        [SerializeField] TextMeshProUGUI _speechBox;
        [SerializeField] GameObject _speechUI;
        [Space(10f)]
        [SerializeField] KeyCode _flipKey = KeyCode.Return;
        [Space(10f)]
        [SerializeField] RectTransform _ImageArea;
        [SerializeField] GameObject _ImageParent;
        static RectTransform _imageArea;
        static GameObject _imageParent;

        public void SetImageArea()
        {
            _imageArea = _ImageArea;
        }
        public void SetImageParent()
        {
            _imageParent = _ImageParent;
        }

        public TextMeshProUGUI SpeakerBox()
        {
            return _speakerBox;
        }
        public TextMeshProUGUI SpeechBox()
        {
            return _speechBox;
        }
        public GameObject SpeechUI()
        {
            return _speechUI;
        }
        public KeyCode FlipKey()
        {
            return _flipKey;
        }
        public static RectTransform ImageArea()
        {
            return _imageArea;
        }
        public static GameObject ImageParent()
        {
            return _imageParent;
        }
    }
    [SerializeField] SpeechManager.Data _managerData;
    [Space(10f)]
    [SerializeField] SpeechCard[] _speechCards;

    byte _index = 0;

    private void Start()
    {
        _managerData.SetImageArea();
        _managerData.SetImageParent();
        Flip();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_managerData.FlipKey()))
        {
            Flip();
        }
    }

    void Flip()
    {
        if( _index < _speechCards.Length)
        {
            _speechCards[_index].PrintSpeech(_managerData.SpeakerBox(), _managerData.SpeechBox());
            ++_index;
            PlayerMovementManager.Enable(false);
        }
        else
        {
            _managerData.SpeakerBox().text = "";
            _managerData.SpeechBox().text = "";
            SpeechCard.ClearImage();
            _managerData.SpeechUI().SetActive(false);
            PlayerMovementManager.Enable(true);
        }
    }

    public void SetCards(SpeechCard[] newCards)
    {
        _managerData.SpeechUI().SetActive(true);
        _speechCards = newCards;
        _index = 0;
        Flip();
    }
}
