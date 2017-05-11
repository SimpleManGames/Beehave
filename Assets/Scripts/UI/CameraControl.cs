using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject world;

    Camera cam;
    Vector3 position;
    Vector2 movementVector;

    public float camMoveSpeed;
    #region Zooming Varialbes
    float zoomSpeed = 5;

    float minX = -360.0f;
    float maxX = 360.0f;

    float minY = -45.0f;
    float maxY = 45.0f;

    float sensX = 155.0f;
    float sensY = 155.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;


    Renderer worldRenderer;

    #endregion

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        worldRenderer = world.GetComponent<Renderer>();
        position = transform.position;

        rotationX = transform.rotation.x;
        rotationY = transform.rotation.y;
        Rotate();
    }

    // Update is called once per frame
    void Update()
    {
        position.x = Mathf.Clamp(transform.position.x, world.transform.position.x - worldRenderer.bounds.size.x / 2.5f, world.transform.position.x + worldRenderer.bounds.size.x / 2.5f);
        position.z = Mathf.Clamp(transform.position.z, world.transform.position.z - worldRenderer.bounds.size.z / 2.5f, world.transform.position.z + worldRenderer.bounds.size.z / 2.5f);
        position.y = Mathf.Clamp(transform.position.y, world.transform.position.y + 2, world.transform.position.y + 7);
    }
    void LateUpdate()
    {
        transform.position = position;
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            position.y += Vector3.up.y * camMoveSpeed * 60 * Time.deltaTime;
            transform.position = position;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            position.y -= Vector3.up.y * camMoveSpeed * 60 * Time.deltaTime;
            transform.position = position;
        }

        movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * camMoveSpeed;

        if (Input.GetButton("Fire2"))
        {
            Rotate();
        }
        if (Input.GetKey(KeyCode.E))
        {
            position.y += Vector3.up.y * camMoveSpeed / 2;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            position.y -= Vector3.up.y * camMoveSpeed / 2;
            transform.position = position;
        }

        transform.position += transform.rotation * new Vector3(movementVector.x, 0, movementVector.y);
    }
    void Rotate()
    {
        rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);
        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}
