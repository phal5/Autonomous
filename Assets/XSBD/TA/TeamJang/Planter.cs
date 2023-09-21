using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    [SerializeField] Transform[] planters;
    [SerializeField] GameObject plant;
    [SerializeField] byte _layer;
    RaycastHit _hit;

    // Update is called once per frame
    void Update()
    {
        foreach(Transform planter in planters)
        {
            Plant(planter);
        }
    }

    void Plant(Transform from)
    {
        Physics.Raycast(from.position, Vector3.down, out _hit);
    }
}
