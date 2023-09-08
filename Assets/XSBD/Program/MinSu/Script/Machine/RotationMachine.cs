using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class RotationMachine : MonoBehaviour
{
    //���� ��ü
    protected Transform controlBoard;
    protected Transform activationField;
    protected Transform moveDetection;
    protected Transform feeder;

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

    private void LoadChildren() //Serialize Field�� ��ȯ ��õ -> Find ���ŭ.
    {
        controlBoard = transform.Find("ControlBoard");
        activationField = transform.Find("ActivationField");
        moveDetection = transform.Find("MoveDetection");
        feeder = transform.Find("Feeder");
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
        LoadChildren();
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