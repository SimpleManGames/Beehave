using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAllHexes : MonoBehaviour
{

    public bool show;

    private void OnValidate()
    {
        if (Grid.Instance != null && Grid.Instance.Hexes != null)
            if (show)
                Grid.Instance.Hexes.ForEach(h => h.transform.GetChild(0).GetComponentInChildren<Renderer>().enabled = true);
            else
                Grid.Instance.Hexes.ForEach(h => h.transform.GetChild(0).GetComponentInChildren<Renderer>().enabled = false);
    }
}
