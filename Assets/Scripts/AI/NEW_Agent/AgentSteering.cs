using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSteering : MonoBehaviour
{
    Vector3 targetPosition;

    public void SetTargetTile(int target)
    {
        targetPosition = Grid.FindHexObject(target).transform.position;
    }
}
