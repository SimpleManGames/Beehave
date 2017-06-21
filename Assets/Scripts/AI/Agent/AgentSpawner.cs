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

    private void Awake()
    {
        spawnerBase = this.GetComponent<AgentBase>(); 
    }

    private void Update()
    {
        if(!spawnerBase.isActive)
        {
            return;
        }

        spawnTimer += Time.fixedDeltaTime;
        
        if(spawnTimer >= spawnGap)
        {
            GameObject spawnedAgentHolder = Instantiate(spawnAgent, this.transform.position, this.transform.rotation);

            spawnAgentBase = spawnedAgentHolder.GetComponent<AgentBase>();
            spawnAgentBase.SetCurrentTile(spawnerBase.currentTileIndex);

            spawnTimer = 0;
        }
    }
}
