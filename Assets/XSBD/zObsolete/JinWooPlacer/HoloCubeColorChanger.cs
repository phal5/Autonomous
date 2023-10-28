using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloCubeColorChanger : MonoBehaviour
{
    [SerializeField] Material redMaterial;
    [SerializeField] Material greenMaterial;
    List<Collider> colliders = new List<Collider>();
    bool canPlace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(colliders.Count != 0)
        {
            GetComponent<MeshRenderer>().material = redMaterial;
            canPlace = false;
        }
        else
        {
            GetComponent<MeshRenderer>().material = greenMaterial;
            canPlace = true;
        }
    }

    public bool canplace()
    {
        return canPlace;
    }

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }

    private void OnEnable()
    {
        colliders = new List<Collider>();
    }
}
