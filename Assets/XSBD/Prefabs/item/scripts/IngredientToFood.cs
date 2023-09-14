using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientToFood : MonoBehaviour
{
    public class Dict
    {
        public string tag;
        public byte _ID;
    }
    public Dict[] Process;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        TryGetComponent<Rigidbody>(out Rigidbody thisRigidbody);
        foreach (Dict dict in Process)
        {
            if (collision.transform.CompareTag(dict.tag))
            {
                FoodManager.InstantiateFoodWithVelocity(dict._ID, transform.position, thisRigidbody.velocity);
            }
        }
    }
}
