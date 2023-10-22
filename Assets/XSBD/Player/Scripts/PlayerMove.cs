using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float smoothRotationTime;
    [SerializeField] private float smoothMoveTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int viewMode; // 1 일때 아이소메트릭 뷰
    [SerializeField] private int jumpspeed;
    [SerializeField] private int maxjumpcount;
    private new Rigidbody rigidbody;
    private int jumpcount;
    private float rotationVelocity;
    private float speedVelocity;
    private float currentSpeed;
    private float targetSpeed;
    private Vector3 zExcecuter = new Vector3(1, 1, 0);

    private Transform cameraTransform;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothMoveTime);
        currentSpeed = targetSpeed;
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
        float fallSpeed = rigidbody.velocity.y;
        //Vector3 velocity = new Vector3(cameraTransform.forward.x * inputDir.x * currentSpeed, 0, currentSpeed * inputDir.y * cameraTransform.forward.z); // 앞방향으로
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        Vector3 right = cameraTransform.right;
        right.y = 0;

        Vector3 velocity = (forward.normalized *inputDir.y + right.normalized*inputDir.x)*currentSpeed;
        velocity.y = fallSpeed;
        rigidbody.velocity = velocity;
        //this.transform.Translate(this.transform.forward * currentSpeed * Time.deltaTime, Space.World);
    }
    void PersonMoveModeJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (jumpcount>0))
        {
            Debug.Log("jump");
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
        PersonMoveMode();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            jumpcount = maxjumpcount;
        }
    }
}
