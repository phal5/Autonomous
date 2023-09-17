using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientProcesser : MonoBehaviour
{
    [Serializable]
    public class Dict
    {
        public string tag;
        public GameObject prefab;
    }
    public Dict[] Process;

    private void OnCollisionEnter(Collision collision)
    {
        TryGetComponent<Rigidbody>(out Rigidbody thisRigidbody);
        foreach(Dict dict in Process)
        {
            if (collision.transform.CompareTag(dict.tag))
            {
                if(dict.prefab != null)
                {
                    GameObject newObject = Instantiate(dict.prefab, this.transform.position, this.transform.rotation);
                    if (newObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                    {
                        rigidbody.velocity = thisRigidbody.velocity;
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.LogError("No rigidbody attached to instanced object!");
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
