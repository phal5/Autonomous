using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseCheck : MonoBehaviour
{
    [SerializeField] private CutterMachine CutterMachine;

    private void OnTriggerEnter(Collider other)
    {
        CutterMachine.Release();
    }
    private void OnTriggerExit(Collider other)
    {
        CutterMachine.ReleaseExit();
    }
}
