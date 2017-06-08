using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Which style do we want to move along the rail by
/// </summary>
public enum PlayBack
{
    Linear,
    Catmull
}

[ExecuteInEditMode]
public class Rail : MonoBehaviour
{
    /// <summary>
    /// The children of this transform  
    /// </summary>
    [Tooltip("Holds info about the children's transform to use as the nodes")]
    public Transform[] nodes;

    public void Start()
    {
        // For some reason GetComponentsInChildren also returns the parent so we have to ignore him
        nodes = GetComponentsInChildren<Transform>().Where(x => x.transform != this.transform).ToArray();
    }

    /// <summary>
    /// Main function for finding the position of the mover
    /// </summary>
    /// <param name="seg">Segment is the current node we last passed use to figure out the next ones</param>
    /// <param name="ratio">How far along are we to the next node based on a 0-1 scale</param>
    /// <param name="play">How we move along the rail</param>
    /// <returns>Vector3 position between current node and the next node</returns>
    public Vector3 Position(int seg, float ratio, PlayBack play)
    {
        switch (play)
        {
            default:
            case PlayBack.Linear:
                return Linear(seg, ratio);
            case PlayBack.Catmull:
                return Catmull(seg, ratio);
        }
    }

    /// <summary>
    /// Calculates the position between the current and next node base off a Linear Curve
    /// </summary>
    private Vector3 Linear(int seg, float ratio)
    {
        Vector3 p1 = nodes[seg].position;
        Vector3 p2 = nodes[seg + 1].position;

        return Vector3.Lerp(p1, p2, ratio);
    }

    /// <summary>
    /// Calculates the position between the current and next node based off a Catmull Curve
    /// </summary>
    private Vector3 Catmull(int seg, float ratio)
    {
        Vector3 p1, p2, p3, p4;

        // Out of bounds checks
        if (seg == 0)
        {
            p1 = nodes[seg].position;
            p2 = p1;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }
        else if (seg == nodes.Length - 2)
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = p3;
        }
        else
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }

        float t2 = ratio * ratio;
        float t3 = t2 * ratio;

        // math i dont understand; the internet said to do this
        float x = 0.5f * ((2.0f * p2.x) + (-p1.x + p3.x) * ratio + (2.0f * p1.x - 5.0f * p2.x + 4.0f * p3.x - p4.x) * t2 + (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x) * t3);
        float y = 0.5f * ((2.0f * p2.y) + (-p1.y + p3.y) * ratio + (2.0f * p1.y - 5.0f * p2.y + 4.0f * p3.y - p4.y) * t2 + (-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y) * t3);
        float z = 0.5f * ((2.0f * p2.z) + (-p1.z + p3.z) * ratio + (2.0f * p1.z - 5.0f * p2.z + 4.0f * p3.z - p4.z) * t2 + (-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z) * t3);

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Calculates the rotation of the mover based on the current nodes rotation and next nodes rotation
    /// </summary>
    public Quaternion Rotation(int seg, float ratio)
    {
        Quaternion q1 = nodes[seg].rotation;
        Quaternion q2 = nodes[seg + 1].rotation;

        return Quaternion.Lerp(q1, q2, ratio);
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < nodes.Length - 1; i++)
        {
#if UNITY_EDITOR
            Handles.color = Color.green;
            Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, 3f);
            Handles.Label(nodes[i].position, (i + 1).ToString());
#endif
        }
    }
}
