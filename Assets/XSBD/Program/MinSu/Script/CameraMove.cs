using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove: MonoBehaviour
{
    public float Yaxis;
    public float Xaxis;
    public Vector3 offset;

    public Transform target; //player

    private float rotSensitive = 3f;
    private float distance = 2f;
    private float RotationMin = -10f;
    private float RotationMax = 85f;
    private float smoothTime = 0.12f;

    
    private Vector3 targetRotation;
    private Vector3 currentVel;

    private void LateUpdate()
    {
        Yaxis = Yaxis + Input.GetAxis("Mouse X") * rotSensitive;
        Xaxis = Xaxis - Input.GetAxis("Mouse Y") * rotSensitive;

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
        this.transform.eulerAngles = targetRotation;

        transform.position = target.position - transform.forward * distance + offset;
    }
}
