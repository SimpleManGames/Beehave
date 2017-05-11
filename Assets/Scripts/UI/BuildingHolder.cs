using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHolder : MonoBehaviour {
    public GameObject Building;
	public void SetObjToPlace()
    {
        if (Interaction.objToPlace == null)
        {
            Interaction.objToPlace = Instantiate(Building);
            for (int i = 0; i < Interaction.objToPlace.GetComponent<Renderer>().materials.Length; i++)
            {
                Interaction.defaultMat.Add(Interaction.objToPlace.GetComponent<Renderer>().materials[i]);
            }
        }
    }
}
