using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMouseDrag : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How fast the models will spin")]
    private float sensitivity;

    // Stores the position of the first mouse click
    private Vector3 mousePosition;
    // Stores the difference between the first mouse click and were the mouse is currently
    private Vector3 mouseOffset;

    // The new rotation vector of the object
    private Vector3 rotation;
    // If we have clicked and held
    private bool isRotating;

    private void Start()
    {
        rotation = Vector3.zero;
    }

    private void Update()
    {
        // If we've clicked and held
        if (isRotating)
        {
            // Calc the difference
            mouseOffset = (Input.mousePosition - mousePosition);

            // Apply diff to the rotation vector
            rotation.y = -(mouseOffset.x + mouseOffset.y) * sensitivity;

            // Act on the rotation
            transform.Rotate(rotation);

            // Set are ref to the were the mouse is at
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