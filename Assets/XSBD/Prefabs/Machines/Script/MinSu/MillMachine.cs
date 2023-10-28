using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillMachine : RotationMachine
{
    [SerializeField]
    private float rotationSpeedMultiplier;
    [SerializeField]
    private GameObject Gear;
    [SerializeField]
    private Transform Crank;
    [SerializeField]
    private GameObject PowerReceiver;
    [SerializeField]
    private GameObject GearCenter;
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
        ActivationAngle = RoundAngle(ActivationAngle);
        if(Crank.position.y < GearCenter.transform.position.y) //최하위값일때 태그 변경
        {
            activationField.tag = "Crusher";
        }
        else
        {
            activationField.tag = "Untagged";
        }
        PowerReceiver.transform.rotation = Quaternion.Euler(0,ActivationAngle,0);
        Gear.transform.rotation = Quaternion.Euler(ActivationAngle, 0,0);

        activationField.transform.position = new Vector3 (activationField.transform.position.x, Crank.position.y, activationField.transform.position.z);
    }
    protected override void Feed()
    {
        Debug.Log("feed");
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
