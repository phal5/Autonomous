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
        Xaxis -= Input.GetAxis("Mouse Y") * rotSensitive;//마우스의 입력을 감도에 맞게 변환

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax); // 입력값의 최댓값과 최솟값으로 한정한다.
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
    }
    void PersonViewCameraRotation()
    {
        ChangeTargetRotation();
        this.transform.eulerAngles = targetRotation;// 각도를 지정해줌

        transform.position = target.position - transform.forward * distance + offset; //각도를 기반으로 카메라의 위치를 조정해준다.
    }
    private void LateUpdate()
    {
        PersonViewCameraRotation();
    }
}
