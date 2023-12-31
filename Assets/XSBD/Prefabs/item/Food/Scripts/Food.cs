using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float _MaxHP;
    float _HP;
    [SerializeField] float Efficiency;
    [SerializeField] GameObject[] Drops;

    // Start is called before the first frame update
    void Start()
    {
        _HP = _MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (_HP < 0)
        {
            Drop();
            Destroy(gameObject);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------

    void Drop()
    {
        foreach(GameObject drop in Drops)
        {
            Instantiate(drop);
        }
    }

    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
    //PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC PUBLIC

    public void DecreaseHP(float amount)
    {
        _HP -= amount;
    }

    public float GetEfficiency()
    {
        return Efficiency;
    }

    public bool IsPickable()
    {
        return _HP == _MaxHP;
    }

    //DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO NOT TOUCH DO
    //[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[
}
