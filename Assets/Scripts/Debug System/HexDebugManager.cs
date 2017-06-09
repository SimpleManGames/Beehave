using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexDebugManager : Singleton<HexDebugManager>
{
    private enum DebugType
    {
        None,
        Index,
        CubeCoord,
        PotentialField
    }
    private DebugType debugType;
    private LayerType layerType;

    private Text layerDebugText;

    [HideInInspector]
    public List<Text> hexSystemText = new List<Text>();

    private bool fadeDone = false;
    private float fadeOutTime = 3f;
    private float currentFadeOutTime = 0f;

    private List<Color> colorsOfMap = new List<Color>();

    private void Start()
    {
        InitColorBlocks();

        layerDebugText = GameObject.Find("Debug Layer Value Text").GetComponent<Text>();
        layerDebugText.text = "";
        hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(false));
    }

    private void Update()
    {
        F1Menu();
        F2Menu();
        FadeTimerCounter();
    }

    public void UpdateValues()
    {
        switch (debugType)
        {
            default:
            case DebugType.None:
                hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(false));
                break;
            case DebugType.Index:
                hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(true));
                hexSystemText.ForEach(h => h.text = h.transform.GetComponentInParent<HexObject>().Index.ToString());
                break;
            case DebugType.CubeCoord:
                hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(true));
                hexSystemText.ForEach(h => h.text = h.transform.GetComponentInParent<HexObject>().hex.cubeCoords.ToString());
                break;
            case DebugType.PotentialField:
                layerDebugText.text += ": " + layerType.ToString();
                hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(true));
                hexSystemText.ForEach(h => h.text = HeatMapInfo.Instance.Field[layerType][h.transform.GetComponentInParent<HexObject>().Index].ToString());
                break;
        }
    }

    private void F1Menu()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            debugType++;
            if (Enum.GetValues(typeof(DebugType)).Length - 1 < (int)debugType)
                debugType = 0;

            layerDebugText.color = Color.white;
            layerDebugText.text = debugType.ToString();

            switch (debugType)
            {
                default:
                case DebugType.None:
                    hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(false));
                    break;
                case DebugType.Index:
                    hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(true));
                    hexSystemText.ForEach(h => h.text = h.transform.GetComponentInParent<HexObject>().Index.ToString());
                    break;
                case DebugType.CubeCoord:
                    hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(true));
                    hexSystemText.ForEach(h => h.text = h.transform.GetComponentInParent<HexObject>().hex.cubeCoords.ToString());
                    break;
                case DebugType.PotentialField:
                    layerDebugText.text += ": " + layerType.ToString();
                    hexSystemText.ForEach(h => h.transform.parent.gameObject.SetActive(true));
                    hexSystemText.ForEach(h => h.text = HeatMapInfo.Instance.Field[layerType][h.transform.GetComponentInParent<HexObject>().Index].ToString());
                    break;
            }
        }
    }

    private void F2Menu()
    {
        if (Input.GetKeyDown(KeyCode.F2) && debugType == DebugType.PotentialField)
        {
            layerDebugText.text = debugType.ToString() + ": ";

            layerType++;
            if (Enum.GetValues(typeof(LayerType)).Length - 1 < (int)layerType)
                layerType = 0;

            layerDebugText.text += layerType.ToString();

            hexSystemText.ForEach(h =>
            {
                Color color = new Color();
                if (layerType != LayerType.Terrain)
                {
                    color = GetColorFromValue(HeatMapInfo.Instance.Field[layerType][h.transform.GetComponentInParent<HexObject>().Index]);
                }
                else
                {
                    if (HeatMapInfo.Instance.Field[layerType][h.transform.GetComponentInParent<HexObject>().Index] != 0)
                    {
                        color = Color.red;
                    }
                }
                h.text = HeatMapInfo.Instance.Field[layerType][h.transform.GetComponentInParent<HexObject>().Index].ToString();
                h.color = color;
            });
        }
    }

    private void FadeTimerCounter()
    {
        if (debugType == DebugType.None)
        {
            if ((currentFadeOutTime += Time.deltaTime) > fadeOutTime)
                StartCoroutine(TextFadeOut());
        }
        else if (fadeDone)
        {
            StopCoroutine(TextFadeOut());
            currentFadeOutTime = 0;
            fadeDone = true;
        }
        else
        {
            StopCoroutine(TextFadeOut());
            currentFadeOutTime = 0;
            fadeDone = true;
        }
    }

    private IEnumerator TextFadeOut()
    {
        float alpha = layerDebugText.color.a;
        fadeDone = false;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / 0.1f)
        {
            layerDebugText.color = new Color(0, 0, 0, Mathf.Lerp(alpha, 0, t));
            yield return null;
        }
        currentFadeOutTime = 0;
        fadeDone = true;
    }

    private void InitColorBlocks()
    {
        colorsOfMap.AddRange(new Color[]{
            new Color(0, 0, 0),// Black
            new Color(0, 0, 1),// Blue
            new Color(0, 1, 1),// Cyan
            new Color(0, 1, 0),// Green
            new Color(1, 1, 0),// Yellow
            new Color(1, 0, 0),// Red
            new Color(1, 1, 1) // White
        });
    }

    public Color GetColorFromValue(double val, double maxVal = 1)
    {
        double valPerc = val / maxVal;
        double colorPerc = 1d / (colorsOfMap.Count - 1);
        double blockOfColor = valPerc / colorPerc;
        int blockIndex = (int)Math.Truncate(blockOfColor);
        double valPercResidual = valPerc - (blockIndex * colorPerc);
        double percOfColor = valPercResidual / colorPerc;

        Color target = colorsOfMap[blockIndex];
        Color next = val == maxVal ? colorsOfMap[blockIndex] : colorsOfMap[blockIndex + 1];

        var deltaR = next.r - target.r;
        var deltaG = next.g - target.g;
        var deltaB = next.b - target.b;

        var R = target.r + (deltaR * percOfColor);
        var G = target.g + (deltaG * percOfColor);
        var B = target.b + (deltaB * percOfColor);

        Color c = colorsOfMap[0];
        try
        {
            c = new Color((byte)R, (byte)G, (byte)B);
        }
        catch { }

        return c;
    }
}