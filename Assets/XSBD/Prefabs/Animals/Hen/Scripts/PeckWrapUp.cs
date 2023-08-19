using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PeckWrapUp : MonoBehaviour
{
    [SerializeField] Rig _chainIK;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WrapUp();
    }

    void WrapUp()
    {
        _chainIK.weight -= Time.deltaTime * 8;
        if (_chainIK.weight == 0)
        {
            this.enabled = false;
        }
    }
}
