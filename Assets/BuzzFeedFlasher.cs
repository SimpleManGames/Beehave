using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuzzFeedFlasher : MonoBehaviour {
    public Image pendingHighlight;
    public GameObject buzzfeedPanel;
    private float currentAlbedo;
    private float flashTime = 1f;
    private bool isFlashing = false;
    void Start()
    {
       // BuzzFeedController.PublishMessage("Hello hello here's your feed");
    }
    void Update()
    {
        if (BuzzFeedController.Instance.messagePending&&!buzzfeedPanel.activeInHierarchy)
        {
            if (!isFlashing) { StartCoroutine(BuzzfeedHighlight()); }
        }
        else
        {
            currentAlbedo = 0;
            pendingHighlight.color = new Vector4(pendingHighlight.color.r, pendingHighlight.color.b, pendingHighlight.color.g, currentAlbedo);
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            BuzzFeedController.Instance.PublishMessage("test");
        }
    }
    IEnumerator BuzzfeedHighlight()
    {
        isFlashing = true;
        currentAlbedo = 255;
        pendingHighlight.color = new Vector4(pendingHighlight.color.r, pendingHighlight.color.b, pendingHighlight.color.g, currentAlbedo);
        yield return new WaitForSeconds(flashTime);
        currentAlbedo = 0;
        pendingHighlight.color = new Vector4(pendingHighlight.color.r, pendingHighlight.color.b, pendingHighlight.color.g, currentAlbedo);
        yield return new WaitForSeconds(flashTime);
        isFlashing = false;
    }
    public void TogglePending()
    {
        BuzzFeedController.Instance.messagePending = false;
    }
}
