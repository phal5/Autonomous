using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float smoothRotationTime;
    [SerializeField] private float smoothMoveTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int viewMode; // 1 일때 아이소메트릭 뷰
    [SerializeField] private int jumpspeed;
    [SerializeField] private int maxjumpcount;
    private int jumpcount;
    private float rotationVelocity;
    private float speedVelocity;
    private float currentSpeed;
    private float targetSpeed;

    private Transform cameraTransform;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        jumpcount = maxjumpcount;
    }

    Vector2 NormalizedInput()
    {
        Vector2 input = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        return input.normalized;
    }
    float RotationAngleBasedOnInputAndCamera(Vector2 inputDir)
    {
        float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        return rotation;
    }
    void RotateSmoothly(float rotationAngle)
    {
        this.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(this.transform.eulerAngles.y, rotationAngle, ref rotationVelocity, smoothRotationTime);
    }
    void ChangeCurrentSpeedBasedOnTargetSpeed(Vector2 inputDir)
    {
        targetSpeed = moveSpeed * inputDir.magnitude; //최종 속도 계산
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothMoveTime);
    }
    void PersonMoveModeHorizontalMove()
    {
        Vector2 inputDir = NormalizedInput();
        if (inputDir != Vector2.zero)
        {
            float rotationAngle = RotationAngleBasedOnInputAndCamera(inputDir);
            RotateSmoothly(rotationAngle);
        }
        ChangeCurrentSpeedBasedOnTargetSpeed(inputDir);
        this.transform.Translate(this.transform.forward * currentSpeed * Time.deltaTime, Space.World);
    }
    void PersonMoveModeJump()
    {
        if (Input.GetButtonDown("Jump") && jumpcount>0)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpspeed, ForceMode.Impulse);
            jumpcount--;
        }
    }
    void PersonMoveMode()
    {
        PersonMoveModeHorizontalMove();
        PersonMoveModeJump();
    }
    void IsometricMoveMode()
    {

    }
    void Update()
    {
        switch (viewMode)
        {
            case 1:
                IsometricMoveMode();
                break;
            default:
                PersonMoveMode();
                break;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            jumpcount = maxjumpcount;
        }
    }
}
