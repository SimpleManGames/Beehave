using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ModelWheel : MonoBehaviour
{
    private List<GameObject> modelsOnWheel = new List<GameObject>();
    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        modelsOnWheel = new List<GameObject>();
        foreach (Transform child in transform)
            modelsOnWheel.Add(child.gameObject);

        points = PointsAroundCircle(modelsOnWheel.Count, 3, transform.position);

        for (int i = 0; i < points.Count; i++)
            modelsOnWheel[i].transform.position = points[i];
    }

    private List<Vector3> PointsAroundCircle(int points, float radius, Vector3 center)
    {
        List<Vector3> retVal = new List<Vector3>();

        if (!Application.isPlaying)
            DebugExtensions.DebugPoint(center, Color.red, .1f, 1);

        float slice = 2 * Mathf.PI / points;
        for (int i = 0; i < points; i++)
        {
            float angle = slice * i;
            float x = (radius * Mathf.Cos(angle) + center.x);
            float z = (radius * Mathf.Sin(angle) + center.z);
            retVal.Add(new Vector3(x, 0, z));
        }

        return retVal;
    }
}
