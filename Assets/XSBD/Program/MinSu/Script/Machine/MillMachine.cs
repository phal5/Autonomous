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

    protected override void Active()
    {
        ActivationAngle = currentAngle * rotationSpeedMultiplier; //작용부 작용각도 계산
        currentHeight = Mathf.Sin(ActivationAngle)*cycleHeight; //높이 Offset 계산
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
        
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Active();
    }
}
