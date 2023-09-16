using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerConverter : MonoBehaviour
{
    Vector3 _beforeRotation;

    // Start is called before the first frame update
    void Start()
    {
        _beforeRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsRotating())
            ConvertFood();

    }

    //  isRotating�� ��� 
    //  �� ��� ���İ� ������ ��� ��ȯ. Ʈ���� ��� 
    bool IsRotating()
    {
        if (transform.rotation.eulerAngles == _beforeRotation)
        {
            return false;
        }
        else
        {
            _beforeRotation = transform.rotation.eulerAngles;
            return true;
        }
        
    }

    void ConvertFood()
    {

    }
    
}
