using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInstancer : MonoBehaviour
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
            InstantiateFood();
            _button = false;
        }
    }

    void InstantiateFood()
    {
        FoodManager.InstantiateFood(0, Vector3.zero);
    }
}
