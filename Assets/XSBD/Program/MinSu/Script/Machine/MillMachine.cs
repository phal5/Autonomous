using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillMachine : Machine
{
    [SerializeField]
    private float rotationSpeedMultiplier;
    [SerializeField]
    private float cycleHeight;
    private float currentHeight;
    private float ActivationAngle;

    
    private float RoundAngle(float angle)
    {
        if (angle > 360f)
        {
            angle -= 360f;
        }
        else if (angle < 0f)
        {
            angle += 360f;
        }
        return angle;
    }
    
    protected override void ChangePreviousVisitedAngle()
    {
        if(currentAngle > previousVisitedAngle)
        {
            previousVisitedAngle += angleDistance;
        }
        else
        {
            previousVisitedAngle -= angleDistance;
        }
        currentAngle = RoundAngle(currentAngle);
        return;
    }

    protected override void Active() //�����丵 �ʿ�5
    {
        ActivationAngle = currentAngle * rotationSpeedMultiplier; //�ۿ�� �ۿ밢�� ���
        currentHeight = Mathf.Sin(ActivationAngle)*cycleHeight; //���� Offset ��� -> normal vector�� foward vector dot production���ε� ��ġ ����(�� ������)
        if(Mathf.Sin(ActivationAngle) < -.8f) //���������϶� �±� ����
        {
            activationField.tag = "Crusher";
        }
        else
        {
            activationField.tag = "Untagged";
        }
        activationField.transform.position = new Vector3 (activationField.transform.position.x, currentHeight + 1.5f,activationField.transform.position.z);
    }
    protected override void Feed()
    {
        //FoodManager.instantiate(byte foodCode , new Vector.zero); ������� ���� ����.
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Active();
    }
}
