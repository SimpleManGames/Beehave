using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    public GameObject spawnAgent;
    public float spawnGap = 5;

    private AgentBase spawnAgentBase;
    private AgentBase spawnerBase;
    private float spawnTimer = 0;

    private void Start()
    {
        spawnerBase = this.GetComponent<AgentBase>(); 
    }

    private void Update()
    {
        spawnTimer += Time.fixedDeltaTime;
        
        if(spawnTimer >= spawnGap)
        {
            GameObject spawnedAgentHolder = Instantiate(spawnAgent);

            spawnAgentBase = spawnedAgentHolder.GetComponent<AgentBase>();
            spawnAgentBase.SetCurrentTile(spawnerBase.currentTileIndex);
        }
    }
}
