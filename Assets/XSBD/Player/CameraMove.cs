using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove: MonoBehaviour
{
    [SerializeField] private float Yaxis;
    [SerializeField] private float Xaxis;
    [SerializeField] private Vector3 personViewOffset;
    [SerializeField] private Vector3 IsometricViewOffset;
    [SerializeField] private Transform target; //player

    private readonly float rotSensitive = 3f;
    private float distance = 2f;
    private float RotationMin = -30f;
    private float RotationMax = 85f;
    private readonly float smoothTime = 0.12f;
    private int mode = 0;

    
    private Vector3 targetRotation;
    private Vector3 currentVel;

    void ChangeTargetRotation()
    {
        Xaxis -= Input.GetAxis("Mouse Y") * rotSensitive;
        if(mode == 0)
        {
            Yaxis += Input.GetAxis("Mouse X") * rotSensitive;
        }
        //���콺�� �Է��� ������ �°� ��ȯ

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax); // �Է°��� �ִ񰪰� �ּڰ����� �����Ѵ�.
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
    }
    void PersonViewCameraRotation()
    {
        ChangeTargetRotation();
        this.transform.eulerAngles = targetRotation;// ������ ��������

        transform.position = target.position - transform.forward * distance + personViewOffset; //������ ������� ī�޶��� ��ġ�� �������ش�.
    }
    void ChangeView()
    {
        if (Input.GetKeyDown(KeyCode.U)) //Ű ���� Input API�� �����ؾ���
        {
            switch (mode)
            {
                case 0: // isometric view
                    Camera.main.orthographic = true;
                    distance = 20f;
                    RotationMin = 15f;
                    RotationMax = 60f;
                    mode = 1;
                    break;
                case 1: // person view
                    Camera.main.orthographic = false;
                    distance = 2f;
                    RotationMin = -30f;
                    RotationMax = 85f;
                    mode = 0;
                    break;
            }
            
        }
    }
    private void Update()
    {
        ChangeView();
    }
    private void LateUpdate()
    {
        PersonViewCameraRotation();
    }
}