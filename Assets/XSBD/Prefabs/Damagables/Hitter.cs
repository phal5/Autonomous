using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Hitter : MonoBehaviour
{
    [Header("Call SetColliderEnability with UnityEvent")]
    [SerializeField] float _damage = 1;
    static Collider[] _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponents<Collider>();
        foreach(Collider collider in _collider)
        {
            collider.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Damagable>(out Damagable damageScript))
        {
            damageScript.DecreaseHP(_damage);
            foreach (Collider collider in _collider)
            {
                collider.enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Damagable>(out Damagable damageScript))
        {
            damageScript.DecreaseHP(_damage);
            foreach (Collider collider in _collider)
            {
                collider.enabled = false;
            }
        }
    }

    public static void SetColliderEnability(bool enable)
    {
        foreach (Collider collider in _collider)
        {
            collider.enabled = enable;
        }
    }
}
