using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMouse : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        Vector3 temp = Input.mousePosition;
        temp.z += 10f;
        transform.position = Camera.main.ScreenToWorldPoint(temp);
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Input.mousePosition, .5f);
    }
}
