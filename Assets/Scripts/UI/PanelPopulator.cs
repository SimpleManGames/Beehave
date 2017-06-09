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

        beeNameTxt.text = "Holder";
        beeAgeTxt.text = "Holder";
        beePhysicalTxt.text = "Holder";
        beeTaskTxt.text = "Holder";
        beeBumblrTxt.text = "Holder";
        //for(int i = 0; i < beeAgent.bumblrText.Count; i++)
        //{
        //    PrintToBumblr(beeAgent.bumblrText[i]);
        //}
    }
    public void PopulateBuilding(GameObject building)
    {

    }
}
