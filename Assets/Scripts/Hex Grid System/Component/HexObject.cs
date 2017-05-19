using System;
using UnityEngine;

[SelectionBase]
[System.Serializable]
public class HexObject : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The position in the arrays")]
    private int index;
    public int Index { get { return index; } set { index = value; } }
    [Tooltip("The Hex this object is tracking")]
    public Hex hex;

    public bool IsCreep { get { return meshRenderer.enabled; } set { meshRenderer.enabled = value; } }

    public bool HasBuilding { get; set; }

    // Used for drawing gizmos
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    /// <summary>
    /// Cast a sphere the size of the hex and see's if we touch anything that should modify the heat map
    /// </summary>
    private void SetupInitFieldData()
    {
        Collider[] objectsToCheck = Physics.OverlapSphere(transform.position, GetComponentInChildren<MeshCollider>().bounds.size.x / 2);

        foreach (var obj in objectsToCheck)
        {
            if (HeatMapInfo.Instance.fieldSetupDictionary.ContainsKey(obj.gameObject.layer))
            {
                var settings = HeatMapInfo.Instance.fieldSetupDictionary[obj.gameObject.layer];

                switch (settings.settings)
                {
                    case HeatMapInfo.DisperseSetting.None:
                        throw new NotImplementedException();
                    case HeatMapInfo.DisperseSetting.Linear:
                        HeatMapInfo.Instance.CalculateLinear(this, settings.type);
                        break;
                    case HeatMapInfo.DisperseSetting.MinValue:
                        HeatMapInfo.Instance.Field[settings.type][Index] = float.MinValue;
                        break;
                    default:
                        return;
                }
            }
        }
    }

    private void Awake()
    {
        meshFilter = transform.FindChild("hex_tile").GetComponentInChildren<MeshFilter>();
        meshRenderer = transform.FindChild("hex_tile").GetComponentInChildren<MeshRenderer>();

        // This is in awake for scene loading
        SetupInitFieldData();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (!meshRenderer.enabled)
            Gizmos.DrawWireMesh(meshFilter.mesh, transform.position, Quaternion.Euler(0, 90, 0));
    }
}