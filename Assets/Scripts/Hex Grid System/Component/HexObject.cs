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

    private bool hasBuilding;
    public bool HasBuilding
    {
        get { return hasBuilding; }
        set { hasBuilding = value; }
    }


    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public void Awake()
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