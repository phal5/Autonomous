using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInstancer : MonoBehaviour
{
    [SerializeField] bool _button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_button)
        {
            InstantiateAnimal();
            _button = false;
        }
    }

    void InstantiateAnimal()
    {
        AnimalManager.InstantiateAnimal(0, transform.position);
    }
}
