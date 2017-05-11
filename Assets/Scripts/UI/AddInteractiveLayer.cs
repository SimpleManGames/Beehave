using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInteractiveLayer : MonoBehaviour {
    public LayerMask interactiveLayer;
	void Awake()
    {
        //gameObject.layer |= interactiveLayer;
        Debug.Log(interactiveLayer.value);
    }
}
