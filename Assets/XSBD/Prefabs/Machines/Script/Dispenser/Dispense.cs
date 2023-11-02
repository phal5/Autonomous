using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispense : MonoBehaviour
{
    [Header("Apply to Gameobject with Trigger")]
    [SerializeField] DispenserInventoryInstance _inventory;
    Collider _collider;
    bool _empty;

    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<Collider>(out _))
        {
            foreach(Collider collider in GetComponents<Collider>())
            {
                if (collider.isTrigger) _collider = collider;
            }
        }
        else
        {
            Debug.LogError(gameObject.name + " has no collider!");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_empty)
        {
            _empty = false;
            _inventory.Dispend(transform);
        }
        _empty = true;
    }

    private void OnTriggerStay(Collider other)
    {
        _empty = false;
    }
}
