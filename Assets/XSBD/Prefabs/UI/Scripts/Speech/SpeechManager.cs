using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _speakerBox;
    [SerializeField] TextMeshProUGUI _speechBox;
    [SerializeField] GameObject _speechUI;
    [SerializeField] KeyCode _flipKey = KeyCode.Return;
    [Space(10f)]
    [SerializeField] SpeechCard[] _speechCards;

    byte _index = 0;

    private void Start()
    {
        Flip();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_flipKey))
        {
            Flip();
        }
    }

    void Flip()
    {
        if( _index < _speechCards.Length)
        {
            _speechCards[_index].PrintSpeech(_speakerBox, _speechBox);
            ++_index;
            PlayerMovementManager.Enable(false);
        }
        else
        {
            _speakerBox.text = "";
            _speechBox.text = "";
            _speechUI.SetActive(false);
            PlayerMovementManager.Enable(true);
        }
    }

    public void SetCards(SpeechCard[] newCards)
    {
        _speechUI.SetActive(true);
        _speechCards = newCards;
        _index = 0;
        Flip();
    }
}
