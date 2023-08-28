using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimal : MonoBehaviour
{
    [SerializeField] Rigidbody _animal;

    // Update is called once per frame
    void Update()
    {
        _animal.velocity = (transform.position - _animal.transform.position);
        _animal.transform.rotation = transform.rotation;
        transform.localPosition = _animal.transform.forward;
    }
}
