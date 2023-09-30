using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class ClearThingsInChildren : MonoBehaviour
{
    [SerializeField] bool _rigidbody = false;
    [SerializeField] bool _monoBehaviour = false;
    [SerializeField] bool _navMeshAgents = false;
    [Space(10f)]
    [SerializeField] bool _execute = false;

    // Update is called once per frame
    void Update()
    {
        if (_execute)
        {
            _execute = false;
            if (_rigidbody)
            {
                ClearRigidbody(transform);
            }
            if (_monoBehaviour)
            {
                ClearBehaviour(transform);
            }
            if (_navMeshAgents)
            {
                ClearNavMeshAgents(transform);
            }
        }
    }

    void ClearRigidbody(Transform transform)
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            DestroyImmediate(rb);
        }
        foreach (Transform child in transform)
        {
            ClearRigidbody(child);
        }
    }

    void ClearBehaviour(Transform transform)
    {
        foreach (MonoBehaviour monoBehaviour in GetComponentsInChildren<MonoBehaviour>())
        {
            DestroyImmediate(monoBehaviour);
        }
        foreach (Transform child in transform)
        {
            ClearBehaviour(child);
        }
    }

    void ClearNavMeshAgents(Transform transform)
    {
        foreach (NavMeshAgent navMeshAgent in GetComponentsInChildren<NavMeshAgent>())
        {
            DestroyImmediate(navMeshAgent);
        }
        foreach(Transform child in transform)
        {
            ClearNavMeshAgents(child);
        }
    }
}
