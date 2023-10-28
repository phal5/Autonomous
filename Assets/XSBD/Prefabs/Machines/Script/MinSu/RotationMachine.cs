using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class RotationMachine : MonoBehaviour
{
    //하위 객체
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

    protected abstract void Active(); //ActivationField의 tag를 변경함으로써 작업을 실행함

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
    protected abstract void ChangePreviousVisitedAngle(); //이전에 방문한 각도를 변환한다. 이때 각도 범위를 넘으면 가장 마지막에 변환해줘야한다.
    protected abstract void Feed(); //Feeder 주변에 음식을 생성함

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