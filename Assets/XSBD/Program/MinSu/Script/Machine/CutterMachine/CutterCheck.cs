using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterCheck : MonoBehaviour
{
    [SerializeField] private CutterMachine CutterMachine;

    private void OnTriggerEnter(Collider other)
    {
        CutterMachine.Cut();
    }
    private void OnTriggerExit(Collider other)
    {
        CutterMachine.CutExit();
    }
}
