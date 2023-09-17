using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Deployer : MonoBehaviour
{
    [SerializeField] GameObject _deployObject;
    
    // Start is called before the first frame update
    void Start()
    {
        Deploy(transform.position, Random.rotation);
    }

    protected void Deploy(Vector3 position, Quaternion rotation)
    {
        Instantiate(_deployObject, position, rotation, transform.parent);
        Destroy(gameObject);
    }
}
