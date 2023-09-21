using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] bool _inverse;
    // Update is called once per frame
    void Update()
    {
        transform.rotation =
            Quaternion.LookRotation((PlayerManager.GetCamPosition() - transform.position) * (_inverse? -1: 1), Vector3.up);
    }
}
