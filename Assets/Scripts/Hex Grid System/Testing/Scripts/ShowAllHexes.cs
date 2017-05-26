using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShowAllHexes : MonoBehaviour
{

    public bool show;

    private void OnValidate()
    {
        if (Grid.Instance != null && Grid.Instance.Hexes != null)
            if (show)
                Grid.Instance.Hexes.ToList().ForEach(h => h.transform.GetChild(0).GetComponentInChildren<Renderer>().enabled = true);
            else
                Grid.Instance.Hexes.ToList().ForEach(h => h.transform.GetChild(0).GetComponentInChildren<Renderer>().enabled = false);
    }
}
