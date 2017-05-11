using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuzzFeedController : MonoBehaviour {
    public static Text buzzFeedMainText;

	// Use this for initialization
	void Start () {
        buzzFeedMainText = GetComponent<Text>();
        buzzFeedMainText.text = "";
        PublishMessage("Hello hello welcome to your BuzzFeed!");
        PublishMessage("Hear about all the happenings of your hive here");
    }

    public static void PublishMessage(string message)
    {
        string currText;
        currText = buzzFeedMainText.text;
        buzzFeedMainText.text = currText + message + "\n";
    }
    public static void ClearFeed()
    {
        buzzFeedMainText.text = "";
    }
}
