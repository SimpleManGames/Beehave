using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingNameController : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    Image namePanel;
    Text text;
	// Use this for initialization
	void Start () {
        namePanel = GetComponentInParent<PanelPopulator>().buildingNamePanel;
        text = namePanel.GetComponentInChildren<Text>();
	}
	
    public void OnPointerEnter(PointerEventData eventData)
    {
        namePanel.enabled = true;
        text.enabled = true;
        namePanel.rectTransform.position = new Vector3(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y+75, GetComponent<RectTransform>().position.z);
        if (GetComponent<BuildingHolder>())
        {
            SetName(GetComponent<BuildingHolder>().Building.GetComponent<BuildingObject>().layerType);
        }
        else
        {
            //TODO: FIX FOR AFTER IFEST
            SetText("Place Creep");
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        namePanel.enabled = false;
        text.enabled = false;
    }
    void SetName(LayerType type)
    {
        switch (type)
        {
            case LayerType.Honey:
                SetText("Honey Production");
                break;
            case LayerType.Sleep:
                SetText("Dwelling");
                break;
            case LayerType.Pollen_Storge:
                SetText("Pollen Storage");
                break;
            case LayerType.Incubator:
                SetText("Incubator");
                break;
            default:
                SetText("Error");
                break;
        }
    }
    void SetText(string name)
    {
        text.text = name;
    }
}
