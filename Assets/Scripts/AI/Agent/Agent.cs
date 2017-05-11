using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum STATS { HUNGER, SLEEP }

public class Agent
{
    // Default constructer, forces an ID for now, will improve later 
    public Agent(int agentID, Tasks startTask, List<Utility<UtilityType>> _agentUtilities, int startTile, bool _living)
    {
        ID = agentID;
        currentTask = startTask;
        utilities = _agentUtilities;
        isLiving = _living;

        currentTileIndex = startTile;
    }

    public bool taskTargetReached = false;
    public bool taskCompleted = false;

    public bool depositingResources = false;

    public bool targetReached = false;

    private Agent tradingAgent;
    private bool isTrading;

    // Agent Index ID for accessing agentList
    public int ID { get; private set; }

    // Change to task Enum when ready
    public Tasks currentTask { get; private set; }

    // Holds the Agents Current TileID
    public int currentTileIndex { get; private set; }

    // Holds x Tiles that the Agent has previously traversed (Amount to be held is equal to tileMemory)
    Queue<int> prevTileIndices = new Queue<int>();
    protected static int tileMemory = 3;

    // Agents held Utilites (Can be Stats or Resources)
    public List<Utility<UtilityType>> utilities;

    // Determines if the Agent can move
    bool isLiving;

    public void Broadcast()
    {
        Grid.EditHeatMapData(currentTileIndex, Task.GetTaskLayer(currentTask));
    }

    public virtual void React()
    {

    }

    public void UpdateUtilities()
    {
        foreach (Utility<UtilityType> utility in utilities)
        {
            utility.growWeight();
        }
    }

    /// <summary>
    /// Evaluates Layers to determine which adjacent tile to move to
    /// </summary>
    /// <param name="adjacencyList">Passing through 6 indices that you will be able to lookup throught DBT function</param>
    public void EvaluateMove()
    {
        // Grab the Adjacent Tiles
        // HeatMapInfo.TileData[] adjacencyList = HeatMapInfo.Instance.AdjacenyList(currentTileIndex);
        Hex[] adjacencyList = Hex.Neighbours(Grid.FindHexObject(currentTileIndex).hex);

        //Holder Values for Tile Weight Checks
        float bestTileWeight = 0;
        float currentTileWeight = 0;

        int bestTileIndex = currentTileIndex;

        // Iterate over each adjacent Tile to currentTileIndex
        foreach (Hex hex in adjacencyList)
        {
            if (Grid.FindHexObject(hex.cubeCoords) != null)
            {
                HexObject tile = Grid.FindHexObject(hex.cubeCoords);

                // Accumulate the layers weight in a total weight for tile
                if (currentTask != Tasks.Null)
                {
                    currentTileWeight += HeatMapInfo.Instance.Field[Task.GetTaskLayer(currentTask)][tile.Index];
                    if(currentTileWeight >= 1f)
                    {
                        Debug.Log("Reached" + currentTask + "Goal");
                        taskTargetReached = true;
                        bestTileIndex = tile.Index;
                        AddPrevTile(currentTileIndex);
                        currentTileIndex = bestTileIndex;
                        ReverseLookup.Instance.MoveAgent(this);
                        return;
                    }
                }
                else currentTileWeight += Random.Range(0.0f, 1.0f);
                currentTileWeight += HeatMapInfo.Instance.Field[LayerType.Terrain][tile.Index];

                //If the tile is better then set best tile to the one currently being worked on
                if (currentTileWeight >= bestTileWeight && Untraversed(tile.Index))
                {
                    bestTileWeight = currentTileWeight;
                    bestTileIndex = tile.Index;
                }

                // Reset the weight holder variable
                currentTileWeight = 0;
            }
        }

        // Set the Agents currentTile to be equal to it's next best choice.
        AddPrevTile(currentTileIndex);
        currentTileIndex = bestTileIndex;
        ReverseLookup.Instance.MoveAgent(this);
    }

    public void EvaluateTask()
    {
        if(taskTargetReached && taskCompleted)
        {
            Debug.Log("Checking for new Task");
            foreach (var utility in utilities)
            {
                if (utility.currentweight <= utility.minWeight && utility.isStat)
                {
                    currentTask = utility.growTask; // Set to task dependant on the Stat that is low (Eat or Sleep for time being)
                    taskTargetReached = false;
                    taskCompleted = false;
                    return;
                }
                else if (!utility.isStat)
                {
                    if (depositingResources)
                    {
                        currentTask = utility.depositTask;
                        taskTargetReached = false;
                        taskCompleted = false;
                    }
                    else currentTask = utility.growTask; taskTargetReached = false; taskCompleted = false;
                    return;
                }
            }
            Debug.Log("Something may be wrong with my decision making" + ID);
            currentTask = Tasks.Null;
        }

        if(taskTargetReached && !taskCompleted)
        {
            switch(currentTask)
            {
                case Tasks.GatherPollen:
                    this.utilities[2].fakeThatShit(10);
                    if(this.utilities[2].currentweight >= this.utilities[2].maxWeight) { taskCompleted = true; depositingResources = true; }
                    break;
                case Tasks.StorePollen:
                    this.utilities[2].fakeThatShit(-10);
                    GlobalResources.Instance.TotalPollen += 10;
                    if (this.utilities[2].currentweight <= this.utilities[2].minWeight) { taskCompleted = true; depositingResources = false; }
                    break;
                case Tasks.Sleep:
                    this.utilities[1].fakeThatShit(10);
                    if (this.utilities[1].currentweight >= this.utilities[1].maxWeight) { taskCompleted = true; }
                    break;
                case Tasks.Eat:
                    this.utilities[0].fakeThatShit(10);
                    GlobalResources.Instance.TotalHoney -= 10;
                    if (this.utilities[0].currentweight >= this.utilities[0].maxWeight) { taskCompleted = true; }
                    break;
            }
                
        }

        //else if(taskTargetReached && !taskCompleted)
        //{
        //    foreach (var utility in utilities)
        //    {
        //        if(utility.isStat && utility.currentweight >= utility.maxWeight && currentTask == utility.growTask)
        //        {
        //            taskCompleted = true;
        //            return;
        //        }
        //        else if(utility.currentweight <= utility.maxWeight && utility.depositTask == currentTask)
        //        {
        //            taskCompleted = true;
        //            return;
        //        }
        //        else if(utility.currentweight >= utility.maxWeight && currentTask == Tasks.GatherPollen)
        //        {
        //            taskCompleted = true;
        //            return;
        //        }
        //    }
        //}
    }

    // Do all the Agent things
    public void Evaluate()
    {
        UpdateUtilities();
        if (isLiving)
        {
            EvaluateTask();
            if (targetReached && !taskTargetReached)
            {
                EvaluateMove();
            }
            //else if (taskTargetReached && !taskCompleted)
            //{
            //    TransferResources();
            //}
        }
    }

    // Checks if a tile is within the Agent tile memory
    bool Untraversed(int index)
    {
        foreach (int prevIndex in prevTileIndices)
        {
            if (index == prevIndex)
            {
                return false;
            }
        }
        return true;
    }

    // Add tile to prevTile Queue
    void AddPrevTile(int tileIndex)
    {
        prevTileIndices.Enqueue(tileIndex);
        if (prevTileIndices.Count() > tileMemory)
        {
            prevTileIndices.Dequeue();
        }
    }

    public int GetLastTile()
    {
        return prevTileIndices.Last();
    }

    public void TransferResources()
    {
        int?[] agentsOnTile = ReverseLookup.Instance.AgentsOnTile(this);

        if (agentsOnTile == null)
        {
            Debug.Log("I think I'm at a Task Point, But I'm Not, Halp" + " " + ID);
        }

        if (!isTrading)
        {
            for (int i = 0; i < 40; i++)
            {
                bool agentIsNull = false;

                if(agentsOnTile[i] == null)
                {
                    agentIsNull = true;
                }

                if(!agentIsNull)
                {
                    Agent holder = Simulation.Instance.GetAgent((int)agentsOnTile[i]);

                    if (!holder.isLiving && (holder.currentTask == Tasks.ProducePollen || holder.currentTask == Tasks.StorePollen))
                    {
                        isTrading = true;
                        tradingAgent = holder;
                        return;
                    }
                }
            }
        }
        else if(tradingAgent.currentTask == Tasks.ProducePollen)
        {
            tradingAgent.utilities[0].transferWeight(this.utilities[3]); Debug.Log("Gathering Pollen");
        }
        else
        {
            this.utilities[2].transferWeight(tradingAgent.utilities[0]); Debug.Log("Storing that Shit");
        }
    }
}