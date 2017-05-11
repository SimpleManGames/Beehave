using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepController : MonoBehaviour {
    static List<Material> creepMats = new List<Material>();
    static List<GameObject> creepSelected = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    static void UpdateCreepMat()
    {
        for(int i = 0; i < creepSelected.Count; i++)
        {
            creepSelected[i].GetComponent<Renderer>().material = creepMats[Random.Range(0, creepMats.Count)];
        }
    }
}
