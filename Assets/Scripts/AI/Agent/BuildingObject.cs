using UnityEngine;

public class BuildingObject : MonoBehaviour, IHighlightable
{

    public LayerType layerType;

    private int agentIndex;
    private int currentTileIndex = 266;

    private AgentInfo info;

    private float spawnTimer = 0;
    public float spawnGap;

    public void OnHighlight()
    {

    }

    public void StoppedHighlighting()
    {

    }

    public void Update()
    {
        if (layerType == LayerType.Incubator)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnGap)
            {
                AgentFactory.Instance.SpawnAgent(currentTileIndex, this.transform);
                spawnTimer = 0;
            }
        }
    }

    public void SetBuildingTile(int placedTile)
    {
        currentTileIndex = placedTile;
        Grid.FindHexObject(placedTile).HasBuilding = true;
    }

    public void CreateAgent()
    {
        info = new AgentInfo();

        switch (layerType)
        {
            case LayerType.Honey:
                agentIndex = Simulation.Instance.MakeAgent(Tasks.ProduceHoney, AgentFactory.Instance.CreateHoneyGenUtilities(), currentTileIndex, false);
                info.genHoneyBuilding();
                break;
            case LayerType.Pollen:
                agentIndex = Simulation.Instance.MakeAgent(Tasks.ProducePollen, AgentFactory.Instance.CreatePollenGenUtilities(), currentTileIndex, false);
                break;
            case LayerType.Pollen_Storge:
                agentIndex = Simulation.Instance.MakeAgent(Tasks.ProducePollen, AgentFactory.Instance.CreatePollenStoreUtilities(), currentTileIndex, false);
                info.genPollenBuilding();
                break;
            case LayerType.Incubator:
                agentIndex = -1;
                info.genSpawner();
                break;
            case LayerType.Sleep:
                agentIndex = Simulation.Instance.MakeAgent(Tasks.Sleep, AgentFactory.Instance.CreateRestUtilities(), currentTileIndex, false);
                break;
        }
    }

    public void Start()
    {
        if(layerType == LayerType.Pollen)
        {
            CreateAgent();
        }
    }
}
