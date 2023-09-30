using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove: MonoBehaviour
{
    [SerializeField] KeyCode _shiftView;
    [SerializeField] private Vector3 personViewOffset;
    [SerializeField] private Vector3 IsometricViewOffset;
    [SerializeField] private Transform target; //player

    float Yaxis;
    float Xaxis;

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
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            Xaxis -= Input.GetAxis("Mouse Y") * rotSensitive;
            Yaxis += Input.GetAxis("Mouse X") * rotSensitive;
        }
        //마우스의 입력을 감도에 맞게 변환

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax); // 입력값의 최댓값과 최솟값으로 한정한다.
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
    }
    void PersonViewCameraRotation()
    {
        ChangeTargetRotation();
        this.transform.eulerAngles = targetRotation;// 각도를 지정해줌

        transform.position = target.position - transform.forward * distance + personViewOffset; //각도를 기반으로 카메라의 위치를 조정해준다.

        if(Physics.SphereCast(target.position + personViewOffset, 0.3f, -transform.forward, out RaycastHit hit, distance))
        {
            if(hit.transform.gameObject.layer == 9)
            {
                transform.position = hit.point + hit.normal * 0.3f;
            }
        }
    }
    void ChangeView()
    {
        if (Input.GetKeyDown(_shiftView)) //키 설정 Input API로 변경해야함
        {
            switch (mode)
            {
                case 0: // isometric view
                    Camera.main.orthographic = false;
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
