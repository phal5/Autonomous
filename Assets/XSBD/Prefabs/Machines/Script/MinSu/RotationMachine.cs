using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class RotationMachine : MonoBehaviour
{
    //���� ��ü
    [SerializeField] protected Transform activationField;
    [SerializeField] protected Transform moveDetection;
    [SerializeField] protected Transform feeder;

    protected float currentSpeed;
    protected float targetSpeed;
    protected float currentAngle = 0f;
    protected float previousVisitedAngle;
    protected float deltaTime;
    [SerializeField]
    protected float angleDistance;
    [SerializeField]
    protected float offset = 2f;

    protected abstract void Active(); //ActivationField�� tag�� ���������ν� �۾��� ������

    protected void CurrentAngleUpdate()
    {
        currentAngle = moveDetection.transform.rotation.eulerAngles.y;
        return;
    } 

    protected void InitializePreviousAngle()
    {
        int quotient = (int)Math.Round(currentAngle / angleDistance);
        if (2*currentAngle - angleDistance *(2*quotient+1) > 0)
        {
            previousVisitedAngle = angleDistance *(quotient+1);
        }
        else
        {
            previousVisitedAngle = angleDistance *quotient;
        }   
    }
    

    protected bool isMovedEnough()
    {
        return Math.Abs(currentAngle - previousVisitedAngle) > (angleDistance - offset);
    }
    protected abstract void ChangePreviousVisitedAngle(); //������ �湮�� ������ ��ȯ�Ѵ�. �̶� ���� ������ ������ ���� �������� ��ȯ������Ѵ�.
    protected abstract void Feed(); //Feeder �ֺ��� ������ ������

    protected virtual void Start()
    {
        CurrentAngleUpdate();
        InitializePreviousAngle();
    }
    protected virtual void Update()
    {
        CurrentAngleUpdate();
        if(isMovedEnough())
        {
            ChangePreviousVisitedAngle();
            Feed();
        }
    }
}