using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float smoothRotationTime;
    public float smoothMoveTime;
    public float moveSpeed;
    private float rotationVelocity;
    private float speedVelocity;
    private float currentSpeed;
    private float targetSpeed;

    private Transform cameraTransform;
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        if(inputDir != Vector2.zero)
        {
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y)*Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref rotationVelocity, smoothRotationTime);
        }
        targetSpeed = moveSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothMoveTime);
        transform.Translate(transform.forward*currentSpeed*Time.deltaTime,Space.World);
    }
}
