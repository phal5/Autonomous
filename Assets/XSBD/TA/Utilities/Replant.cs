using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Replant : MonoBehaviour
{
    RaycastHit _hit;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _hit))
        {
            Quaternion rotation =
                Quaternion.LookRotation(_hit.normal, Vector3.back)
                * Quaternion.Euler(Vector3.right * 90)
                * Quaternion.Euler(Vector3.up * Random.value * 360);
            transform.rotation = rotation;
            transform.position = _hit.point;
            DestroyImmediate(this);
        }
    }
}
