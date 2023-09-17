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
        // �κ��丮 Ȯ�� �� ���� ������,
        // ��ġ�� ���� �ν��Ͻÿ���Ʈ
        // �κ��丮���� ���� �ϳ� ���ֱ�
    }
    private void FeederBackFoodInitiate()
    {
        // �κ��丮 Ȯ�� �� ���� ������,
        // ��ġ�� ���� �ν��Ͻÿ���Ʈ
        // �κ��丮���� ���� �ϳ� ���ֱ�
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
