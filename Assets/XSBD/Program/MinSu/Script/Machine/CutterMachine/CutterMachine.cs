using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterMachine : MonoBehaviour
{
    [SerializeField] private GameObject FeederFront;
    [SerializeField] private GameObject FeederBack;
    [SerializeField] private GameObject CutterBoard;
    [SerializeField] private GameObject Blade;
    [SerializeField] private float ReleaseRotationSpeed;
    [SerializeField] private float RetractRotationSpeed;
    [SerializeField] private float maxtargetRotation;
    private float targetRotation = 0; 

    public void Cut()
    {
        Blade.tag = "Cutter";
        FeederBackFoodInitiate();
    }
    public void CutExit()
    {
        Blade.tag= "Untagged";
    }
    public void Release()
    {
        FeederFrontFoodInitiate();
        targetRotation = maxtargetRotation;
    }
    public void ReleaseExit()
    {
        targetRotation = 0f;
    }
    private void FeederFrontFoodInitiate()
    {
        // 인벤토리 확인 후 음식 있으면,
        // 위치에 음식 인스턴시에이트
        // 인벤토리에서 음식 하나 없애기
    }
    private void FeederBackFoodInitiate()
    {
        // 인벤토리 확인 후 음식 있으면,
        // 위치에 음식 인스턴시에이트
        // 인벤토리에서 음식 하나 없애기
    }

    private void RotateToTarget()
    {
        if(CutterBoard.transform.rotation.eulerAngles.z > 300)
        {
            CutterBoard.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if ((targetRotation == maxtargetRotation) && (CutterBoard.transform.rotation.eulerAngles.z < targetRotation))
        {
            CutterBoard.transform.Rotate(0,0,ReleaseRotationSpeed);
        }
        else if(targetRotation == 0 && (CutterBoard.transform.rotation.z > targetRotation))
        {
            CutterBoard.transform.Rotate(0,0,-RetractRotationSpeed);
        }
    }

    void Update()
    {
        RotateToTarget();
    }
}
