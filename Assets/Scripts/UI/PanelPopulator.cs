using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Fix this when Agent is completed

public class PanelPopulator : Singleton<PanelPopulator>
{
    public Image buildingNamePanel;
    #region Bumblr Texts
    public Text beeNameTxt;
    public Text beeAgeTxt;
    public Text beeBumblrTxt;
    public Text beePhysicalTxt;
    public Text beeTaskTxt;
    #endregion
    #region Building Texts
    public Text buildingNameTxt;
    public Text buildingDescriptionTxt;
    public Text buildingOccupancyTxt;
    #endregion
    public void PrintToBumblr(string message)
    {
        string currText;
        currText = beeBumblrTxt.text;
        beeBumblrTxt.text = currText + message + "\n";
    }
    public void PopulateBumblr(GameObject bee)
    {
        //populate with agent stuff
        AgentBase beeAgent = bee.GetComponent<AgentBase>();
        AgentBrain beeBrain = bee.GetComponent<AgentBrain>();

        beeNameTxt.text = beeAgent.info.name;
        beeAgeTxt.text = beeAgent.info.capacity.ToString();
        beePhysicalTxt.text = beeAgent.info.trait;
        beeTaskTxt.text = beeBrain.currentTask.type.ToString();
        beeBumblrTxt.text = "BumblrText";
        //for(int i = 0; i < beeAgent.bumblrText.Count; i++)
        //{
        //    PrintToBumblr(beeAgent.bumblrText[i]);
        //}
    }
    public void PopulateBuilding(GameObject building)
    {
        AgentBase buildingAgent = building.GetComponent<AgentBase>();

        buildingNameTxt.text = buildingAgent.info.name;
        buildingDescriptionTxt.text = buildingAgent.info.description;
        buildingOccupancyTxt.text = buildingAgent.info.capacity.ToString();
    }
}
