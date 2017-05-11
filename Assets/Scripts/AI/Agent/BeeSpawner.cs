using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    private float spawnTimer = 0;
    public float spawnGap;
    public int spawnTileIndex;

    AgentInfo spawnerInfo;

    public void Update()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= spawnGap)
        {
            AgentFactory.Instance.SpawnAgent(spawnTileIndex, this.transform);
            spawnTimer = 0;
        }
    }
}
