using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float mouseSpeed;
    public float upDownSpeed;
    public float horizontalSpeed;
    private float cameraYdelta;
    private float cameraX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get the mouse inputs
        cameraYdelta = Input.GetAxis("Mouse X") * mouseSpeed;
        cameraX += Input.GetAxis("Mouse Y") * mouseSpeed;
        // clamp the vertical rotation
        cameraX = Mathf.Clamp(cameraX, -90f, 90f);
        // rotate the camera
        transform.eulerAngles = new Vector3(-cameraX, transform.eulerAngles.y + cameraYdelta, 0);

        //wasd for moving
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * upDownSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += Vector3.down * upDownSpeed * Time.deltaTime;
        }
        transform.position += (Vector3.Normalize(Vector3.ProjectOnPlane(transform.right, Vector3.up)) * Input.GetAxisRaw("Horizontal") + 
            Vector3.Normalize(Vector3.ProjectOnPlane(transform.forward, Vector3.up)) * Input.GetAxisRaw("Vertical")) * horizontalSpeed * Time.deltaTime;
    }
}
