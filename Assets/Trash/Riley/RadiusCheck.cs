using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RadiusCheck : MonoBehaviour {

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetComponent<MeshFilter>().mesh.bounds.center, GetComponent<MeshFilter>().mesh.bounds.extents.x);
    }
}
