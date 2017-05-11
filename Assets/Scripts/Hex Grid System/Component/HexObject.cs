using System;
using UnityEngine;

[SelectionBase]
[System.Serializable]
public class HexObject : MonoBehaviour
{
    [SerializeField]
    private int index;
    public int Index { get { return index; } set { index = value; } }
    public Hex hex;

    public bool IsCreep { get { return meshRenderer.enabled; } set { meshRenderer.enabled = value; } }

    public bool HasBuilding { get; set; }

    // Used for drawing gizmos
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void SetupInitFieldData() {
        Collider[] objectsToCheck = Physics.OverlapSphere(transform.position, GetComponentInChildren<MeshCollider>().bounds.size.x / 2);

        foreach (var obj in objectsToCheck)
            if (HeatMapInfo.Instance.initFieldSetupDictionary.ContainsKey(obj.gameObject.layer))
            {
                var settings = HeatMapInfo.Instance.initFieldSetupDictionary[obj.gameObject.layer];
         
                switch (settings.settings)
                {
                    case HeatMapInfo.DisperseSetting.None:
                        throw new NotImplementedException();
                    case HeatMapInfo.DisperseSetting.Linear:
                        HeatMapInfo.Instance.CalculateLinear(hex, settings);
                        break;
                    case HeatMapInfo.DisperseSetting.MinValue:
                        HeatMapInfo.Instance.Field[settings.type][Index] = float.MinValue;
                        break;
                    default:
                        return;
                }
            }
    }

    public void Start()
    {
        meshFilter = transform.GetChild(0).GetComponentInChildren<MeshFilter>();
        meshRenderer = transform.GetChild(0).GetComponentInChildren<MeshRenderer>();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(!meshRenderer.enabled)
            Gizmos.DrawWireMesh(meshFilter.mesh, transform.position, Quaternion.Euler(0,90,0));
    }
}