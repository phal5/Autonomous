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

    protected bool Deploy(Vector3 position, Quaternion rotation)
    {
        if(Instantiate(_deployObject, position, rotation, transform.parent) != null)
        {
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
}
