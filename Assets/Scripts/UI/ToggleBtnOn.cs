using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBtnOn : MonoBehaviour {
    
    public void CheckToggle(GameObject childPopup)
    {
        if(childPopup.activeInHierarchy)
        {
            childPopup.SetActive(false);
        }
        else { childPopup.SetActive(true); }
    }
}
