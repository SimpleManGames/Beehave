using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMouseDrag : MonoBehaviour
{
    [SerializeField]
    private float sensitivity;

    private Vector3 mousePosition;
    private Vector3 mouseOffset;

    private Vector3 rotation;
    private bool isRotating;

    private void Start()
    {
        rotation = Vector3.zero;
    }

    private void Update()
    {
        if (isRotating)
        {
            mouseOffset = (Input.mousePosition - mousePosition);

            rotation.y = -(mouseOffset.x + mouseOffset.y) * sensitivity;

            transform.Rotate(rotation);

            mousePosition = Input.mousePosition;
        }
    }

    private void OnMouseDown()
    {
        isRotating = true;
        mousePosition = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        isRotating = false;
    }
}