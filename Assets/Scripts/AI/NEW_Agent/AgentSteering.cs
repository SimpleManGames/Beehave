using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSteering : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 2f;

    public const float goalDistance = .5f;

    public int targetTileIndex = 0;
    Vector3 targetPosition;

    public bool targetReached { get; private set; }

    public void SetTargetTile(int target)
    {
        targetTileIndex = target;
        targetPosition = Grid.FindHexObject(target).transform.position;
    }

    public void Move()
    {
        // Checks to see if the Agent has reached their target
        if (Vector3.Distance(this.transform.position, targetPosition) < goalDistance)
        {
            targetReached = true;
        }
        else targetReached = false;

        // Smoothly rotates and moves the Agent transform towards its target
        if (!targetReached)
        {
            Vector3 targetDir = targetPosition - transform.position;
            float step = rotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, .5f);

            transform.rotation = Quaternion.LookRotation(newDir);

            transform.position += (targetPosition - transform.position) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(!targetReached)
        {
            Move();
        }
    }
}
