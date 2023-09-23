using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Hitter : MonoBehaviour
{
    [SerializeField] KeyCode _key = KeyCode.Mouse0;

    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if(Input.GetKeyDown(_key)) 
        {
            _collider.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("entered");
        if (other.gameObject.TryGetComponent<Damagable>(out Damagable damageScript))
        {
            damageScript.DecreaseHP();
            print("Work");
        }

        _collider.enabled = false;
    }
}
