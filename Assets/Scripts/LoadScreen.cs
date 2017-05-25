using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Loads a scene on start
/// </summary>
public class LoadScreen : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The name of the scene to be loaded")]
    private string sceneName;
    [SerializeField]
    [Tooltip("The Text object that will be displayed to the user")]
    private Text loadingText;
    [SerializeField]
    [Tooltip("The color of the 'loadingText'")]
    private Color loadingColor;

    [SerializeField]
    [Tooltip("The duration between no alpha and max alpha")]
    private float duration = 1.0f;

    /// <summary>
    /// Returns LoadingColor with an alpha of 0
    /// </summary>
    private Color LoadingColorNoAlpha
    {
        get { return new Color(loadingColor.r, loadingColor.g, loadingColor.b, 0); }
    }

    private void Start()
    {
        // Start the screen loading using an async operation
        StartCoroutine(LoadNewScene());
    }

    private void Update()
    {
        // Player feed back to tell them if the scene is stilling loading and not frozen
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        loadingText.color = Color.Lerp(loadingColor, LoadingColorNoAlpha, lerp);
    }

    /// <summary>
    /// Loads the scene defined by sceneName as an Async Operation
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadNewScene()
    {
        // Have a delay at the beginning
        yield return new WaitForSeconds(3f);

        // Create variables
        AsyncOperation async;
        
        // Check if we have a scene to go to
        if (!string.IsNullOrEmpty(sceneName))
            // Init the AsyncOperation
            async = SceneManager.LoadSceneAsync(sceneName);
        else
        {
            // Alert the user if we don't have a scene
            Debug.LogError("LoadNewScene sceneName is Empty or Null!", this);
            async = null;
        }

        // Check to see if we init the operation
        if (async != null)
            // Keep running until it's done
            while (!async.isDone)
                yield return null;
        else
            // Alert the user if the async operation isn't set
            Debug.LogWarning("Can't load new scene due to AsyncOperation being null", this);

        yield return null;
    }
}
