using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove: MonoBehaviour
{
    [SerializeField] private float Yaxis;
    [SerializeField] private float Xaxis;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target; //player

    private float rotSensitive = 3f;
    private float distance = 2f;
    private float RotationMin = -30f;
    private float RotationMax = 85f;
    private float smoothTime = 0.12f;

    
    private Vector3 targetRotation;
    private Vector3 currentVel;

    void ChangeTargetRotation()
    {
        Yaxis += Input.GetAxis("Mouse X") * rotSensitive;
        Xaxis -= Input.GetAxis("Mouse Y") * rotSensitive;//���콺�� �Է��� ������ �°� ��ȯ

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax); // �Է°��� �ִ񰪰� �ּڰ����� �����Ѵ�.
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
    }
    void PersonViewCameraRotation()
    {
        ChangeTargetRotation();
        this.transform.eulerAngles = targetRotation;// ������ ��������

        transform.position = target.position - transform.forward * distance + offset; //������ ������� ī�޶��� ��ġ�� �������ش�.
    }
    private void LateUpdate()
    {
        PersonViewCameraRotation();
    }
}
