using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class MouseHighlight : MonoBehaviour
{

    #region Variables

#if UNITY_EDITOR

    public enum DEBUG_LEVEL { None, Basic, Explicit }

    public DEBUG_LEVEL debug;

#endif

    Camera highlightCam;

    List<LayerMask> prevLayers;
    LayerMask highlightLayer;

    Transform highlightedObject;

    #endregion

    #region Functions

    #region Unity

    // Use this for initialization
    void Start ()
    { GetComponents(); SetDefaultLayers(); }
	
	// Update is called once per frame
	void Update ()
    { SetHighlight(); }

    #endregion

    #region Highlight

    void SetHighlight()
    {
        RaycastHit hitInfo = HighlightCheck();
        if (hitInfo.transform != null && hitInfo.transform != highlightedObject && hitInfo.transform.gameObject.GetComponent(typeof(IHighlightable)))
        {
            if(highlightedObject != null)
                ResetHighlightedObject();
            SetHighlightedObject(hitInfo.transform);
        }
        else if (hitInfo.transform != highlightedObject)
            ResetHighlightedObject();
    }

    RaycastHit HighlightCheck()
    {
        RaycastHit hitInfo;
        Ray ray = highlightCam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitInfo);

#if UNITY_EDITOR

        if (debug == DEBUG_LEVEL.Explicit)
        
            if (hitInfo.transform != null) Debug.Log("Component 'MouseHighlight' on object '" + transform.name +
                                                         "' has hit '" + hitInfo.transform.name + "' in 'HighlightCheck'!");
            else
                Debug.Log("Component 'MouseHighlight' on object '" + transform.name + "' didn't hit anything in 'HighlightCheck'!");

#endif

        return hitInfo;
    }

    void ResetHighlightedObject()
    {
        if(highlightedObject != null)
        {
            highlightedObject.gameObject.layer = prevLayers[0];
            for (int i = 1; i < prevLayers.Count; ++i)
                highlightedObject.GetChild(i - 1).gameObject.layer = prevLayers[i];

#if UNITY_EDITOR

            if (debug == DEBUG_LEVEL.Basic) Debug.Log(highlightedObject.name + " is no longer highlighted!");

#endif

            prevLayers = null;
            highlightedObject.GetComponent<IHighlightable>().StoppedHighlighting();
            highlightedObject = null;
        }
    }

    void SetHighlightedObject(Transform obj)
    {
        highlightedObject = obj;
        prevLayers = new List<LayerMask>();
        prevLayers.Add(obj.gameObject.layer);
        foreach (Transform t in obj)
            prevLayers.Add(t.gameObject.layer);
        obj.gameObject.layer = highlightLayer;
        foreach (Transform t in obj)
            t.gameObject.layer = highlightLayer;
        obj.GetComponent<IHighlightable>().OnHighlight();

#if UNITY_EDITOR

        if (debug == DEBUG_LEVEL.Basic) Debug.Log(highlightedObject.name + " is now highlighted!");

#endif

    }

    #endregion

    #region Setup

    void SetDefaultLayers()
    { highlightLayer = LayerMask.NameToLayer("Highlighted"); }

    void GetComponents()
    { highlightCam = GetComponent<Camera>(); }

#endregion

#endregion

}
