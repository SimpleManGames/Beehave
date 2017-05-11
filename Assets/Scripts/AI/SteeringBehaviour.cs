using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    // Arbitrary Movement Speeds
    public float rotationSpeed = 10f;
    public float movementSpeed = 10f;

    Agent attachedAgent;

    // Determines if Agent has reached its Target
    private bool targetReached = false;

    // Arbitrary Distance when Agent is considered to have Reached their target
    public float targetDistance = .1f;

    // Transform of the Agents Target Tile
    public Transform targetTransform = null;

    // Sets Agent Target and moves towards it
    void moveTowards()
    {
        targetTransform = setTarget();
        // Checks to see if the Agent has reached their target
        if (Vector3.Distance(this.transform.position, targetTransform.position) < targetDistance)
            targetReached = true;
        else targetReached = false;

        // Smoothly rotates and moves the Agent transform towards its target
        if (!targetReached)
        {
            Vector3 targetDir = targetTransform.position - transform.position;
            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, .5f);
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.position += (targetTransform.position - transform.position) * Time.deltaTime;
        }
    }

    // Sets the Target based on Agents Chosen Tile Index
    Transform setTarget()
    {
        HexObject targetHex = Grid.FindHexObject(attachedAgent.currentTileIndex);
        return targetHex.transform;
    }

    public void Start()
    {
        attachedAgent = GetComponent<AgentObject>().agent;
    }

    public void Update()
    {
        try
        {
            if (attachedAgent == null)
                attachedAgent = GetComponent<AgentObject>().agent;
        }
        catch
        {
            Debug.Log(attachedAgent);
        }
        attachedAgent.targetReached = targetReached;
        moveTowards();
    }
}