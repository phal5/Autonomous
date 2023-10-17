using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Hitter : MonoBehaviour
{
    [SerializeField] int _mouseButtonIndex = 0;

    static Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Damagable>(out Damagable damageScript))
        {
            damageScript.DecreaseHP();
        }

        _collider.enabled = false;
    }

    public static void SetColliderEnability(bool enable)
    {
        _collider.enabled = enable;
    }
}
