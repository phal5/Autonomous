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

    protected override void Active() //리팩토링 필요5
    {
        ActivationAngle = currentAngle * rotationSpeedMultiplier; //작용부 작용각도 계산
        currentHeight = Mathf.Sin(ActivationAngle)*cycleHeight; //높이 Offset 계산 -> normal vector와 foward vector dot production으로도 대치 가능(더 저렴함)
        if(Mathf.Sin(ActivationAngle) < -.8f) //최하위값일때 태그 변경
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
        //FoodManager.instantiate(byte foodCode , new Vector.zero); 방식으로 생성 가능.
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
