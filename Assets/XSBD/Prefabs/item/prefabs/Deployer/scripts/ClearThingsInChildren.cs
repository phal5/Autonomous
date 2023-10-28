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
    [SerializeField] bool _meshColliders = false;
    [Space(10f)]
    [SerializeField] bool _execute = false;

    // Update is called once per frame
    void Update()
    {
        if (_execute)
        {
            _execute = false;
            foreach (Transform child in transform)
            {
                if (_rigidbody)
                {
                    ClearRigidbody(child);
                }
                if (_monoBehaviour)
                {
                    ClearBehaviour(child);
                }
                if (_navMeshAgents)
                {
                    ClearNavMeshAgents(child);
                }
                if (_meshColliders)
                {
                    ClearMeshColliders(child);
                }
            }
        }
    }

    void ClearRigidbody(Transform transform)
    {
        foreach (Rigidbody rb in transform.GetComponents<Rigidbody>())
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
        foreach (MonoBehaviour monoBehaviour in transform.GetComponents<MonoBehaviour>())
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
        foreach (NavMeshAgent navMeshAgent in transform.GetComponents<NavMeshAgent>())
        {
            DestroyImmediate(navMeshAgent);
        }
        foreach(Transform child in transform)
        {
            ClearNavMeshAgents(child);
        }
    }

    void ClearMeshColliders(Transform transform)
    {
        foreach (MeshCollider meshCollider in transform.GetComponents<MeshCollider>())
        {
            DestroyImmediate(meshCollider);
        }
        foreach (Transform child in transform)
        {
            ClearMeshColliders(child);
        }
    }
}
