using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuzzFeedController : Singleton<BuzzFeedController> {
    public Text buzzFeedMainText;
    public bool messagePending = false;

    public void PublishMessage(string message)
    {
        string currText;
        currText = buzzFeedMainText.text;
        buzzFeedMainText.text = currText + message + "\n";
        messagePending = true;
    }
    public void ClearFeed()
    {
        buzzFeedMainText.text = "";
    }
   
}
