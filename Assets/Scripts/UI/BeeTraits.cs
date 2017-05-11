using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeeTraits
{

    public static List<string> beeNames = new List<string>() { "Beeverly", "Been", "Beetoven", "Buzz", "Beeatrice",
                                                        "Beengo", "Beeau", "Beeba", "Beeca", "Beechira", "Beeck",
                                                        "Beeckett", "Beeacon", "Beeaman", "Beeasley", "Beeaufort",
                                                        "Beeckham","Beenadict","Beell","Beernie","Beenie","Beenji",
                                                        "Beennett","Beenito","Beellamy","Beelmont","Beelton","Beerkly",
                                                        "Beenson","Beerlin","Beernard","Beerry","Beert","Beeyonce"};

    public static List<string> physicalTraits = new List<string>() { "Swole", "Scrawny", "Nimble", "Clutsy","Plump","Petite",
                                                            "Large Tarsal Claws","Small Tarsal Claws" };

    public static List<string> socialTraits = new List<string>() { "Kind","Rude","Greedy","Selfless","Anxious","Exuberant","Fearless",
                                                            "Observant","Intelligent","Capable","Charming","Dutiful","Trusting",
                                                            "Lazy","Finicky","Arrogant","Quarrelsome","Unruly","Obnoxious" };

    public static Dictionary<LayerType, string> buildingDescriptions;
}
