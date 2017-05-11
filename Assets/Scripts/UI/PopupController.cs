using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupController : MonoBehaviour {
    public List<GameObject> popups = new List<GameObject>();
    public List<GameObject> secondaryPopups = new List<GameObject>();
    public List<GameObject> panels = new List<GameObject>();

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                CloseAllPopups();
            }
        }
    }
    public void ActiveChild()
    {
        
    }
    public void CloseAllPopups()
    {
        CloseAllSecondaryPopups();
        for (int i = 0; i < popups.Count; i++)
        {
            if (popups[i].activeInHierarchy)
            {
                popups[i].SetActive(false);
            }
        }
        //for (int i =0; i < panels.Count;i++)
        //{
        //    if (panels[i].activeInHierarchy)
        //    {
        //        panels[i].SetActive(false);
        //    }
        //}
    }
    public void CloseAllPopups(GameObject objToExclude)
    {
        CloseAllSecondaryPopups();
        for (int i = 0; i < popups.Count; i++)
        {
            if (popups[i].activeInHierarchy)
            {
                if (popups[i]!= objToExclude)
                popups[i].SetActive(false);
            }
        }
        //for (int i = 0; i < panels.Count; i++)
        //{
        //    if (panels[i].activeInHierarchy)
        //    {
        //        if(panels[i]!=objToExclude)
        //        panels[i].SetActive(false);
        //    }
        //}
    }
    public void CloseAllSecondaryPopups()
    {
        for (int i = 0; i < secondaryPopups.Count; i++)
        {
            if (secondaryPopups[i].activeInHierarchy)
            {
                secondaryPopups[i].SetActive(false);
            }
        }
    }
    public void CloseAllSecondaryPopups(GameObject obj)
    {
        for (int i = 0; i < secondaryPopups.Count; i++)
        {
            if (secondaryPopups[i].activeInHierarchy)
            {
                if (secondaryPopups[i]!= obj)
                secondaryPopups[i].SetActive(false);
            }
        }
    }
}
