using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable] public class SpeechCard
{
    [SerializeField] string _speaker;
    [SerializeField] string _speech;

    public void PrintSpeech(TextMeshProUGUI speaker, TextMeshProUGUI speech)
    {
        speaker.text = _speaker;
        speech.text = _speech;
    }
}
