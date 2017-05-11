using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentFactory : Singleton<AgentFactory>
{
    public GameObject beePrefab;

    public List<Utility<UtilityType>> CreateLivingUtilities()
    {
        List<Utility<UtilityType>> livingUtilities = new List<Utility<UtilityType>>();

        Utility<UtilityType> hungerUtility = new Utility<UtilityType>(0, UtilityType.Hunger, UtilityType.Honey, Tasks.Eat, Tasks.Null, 100, -1, true);
        Utility<UtilityType> sleepUtility = new Utility<UtilityType>(0, UtilityType.Sleep, UtilityType.Rest, Tasks.Sleep, Tasks.Null, 100, -1, true);
        Utility<UtilityType> pollenUtility = new Utility<UtilityType>(2, UtilityType.Pollen, UtilityType.Pollen, Tasks.GatherPollen, Tasks.StorePollen, 0, 1);

        livingUtilities.Add(hungerUtility);
        livingUtilities.Add(sleepUtility);
        livingUtilities.Add(pollenUtility);

        return livingUtilities;
    }

    public List<Utility<UtilityType>> CreatePollenGenUtilities()
    {
        List<Utility<UtilityType>> storageUtilites = new List<Utility<UtilityType>>();

        Utility<UtilityType> pollenUtility = new Utility<UtilityType>(10, UtilityType.Pollen, UtilityType.Pollen, Tasks.ProducePollen, Tasks.Null, 100, 20 );

        storageUtilites.Add(pollenUtility);

        return storageUtilites;
    }

    public List<Utility<UtilityType>> CreatePollenStoreUtilities()
    {
        List<Utility<UtilityType>> storageUtilites = new List<Utility<UtilityType>>();

        Utility<UtilityType> pollenUtility = new Utility<UtilityType>(10, UtilityType.Pollen, UtilityType.Pollen, Tasks.ProducePollen, Tasks.Null, 0, 0);

        storageUtilites.Add(pollenUtility);

        return storageUtilites;
    }

    public List<Utility<UtilityType>> CreateHoneyGenUtilities()
    {
        List<Utility<UtilityType>> storageUtilites = new List<Utility<UtilityType>>();

        Utility<UtilityType> honeyUtility = new Utility<UtilityType>(5, UtilityType.Honey, UtilityType.Honey, Tasks.ProduceHoney, Tasks.Null, 100, 10);

        storageUtilites.Add(honeyUtility);

        return storageUtilites;
    }

    public List<Utility<UtilityType>> CreateRestUtilities()
    {
        List<Utility<UtilityType>> storageUtilites = new List<Utility<UtilityType>>();

        Utility<UtilityType> restUtility = new Utility<UtilityType>(10, UtilityType.Rest, UtilityType.Rest, Tasks.Sleep, Tasks.Null, 100, 10);

        storageUtilites.Add(restUtility);

        return storageUtilites;
    }

    public void SpawnAgent(int spawnTile, Transform spawnerLocation)
    {
        GameObject babooBee = GameObject.Instantiate(beePrefab, spawnerLocation.position, spawnerLocation.rotation);
        AgentObject babooBeeAgent = babooBee.GetComponent<AgentObject>();
        babooBeeAgent.CreateAgent(spawnTile, Tasks.GatherPollen);
    }
}
