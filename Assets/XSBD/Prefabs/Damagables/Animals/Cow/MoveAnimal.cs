using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAnimal : MonoBehaviour
{
    [SerializeField] Rigidbody _animal;
    [SerializeField] NavMeshAgent _agent;
    private void Start()
    {
        if (!_agent)
        {
            _agent = GetComponent<NavMeshAgent>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        _animal.transform.position = Vector3.Scale(transform.position, Vector3.one - Vector3.up) + _animal.transform.position.y * Vector3.up;
        _animal.transform.rotation = transform.rotation;
        transform.position = _animal.transform.position;
    }
}
