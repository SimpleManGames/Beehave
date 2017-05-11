using UnityEngine.UI;

public class GlobalResources : Singleton<GlobalResources>
{
    private int totalPollen;
    public int TotalPollen
    {
        get { return totalPollen; }
        set
        {
            SetPollen(value);
            totalPollen = value;
        }
    }
    private int totalHoney;
    public int TotalHoney
    {
        get { return totalHoney; }
        set
        {
            SetHoney(value);
            totalHoney = value;
        }
    }
    private int totalPopulation;
    public int TotalPopulation
    {
        get { return totalPopulation; }
        set
        {
            SetPopulation(value, populationCap);
            totalPopulation = value;
        }
    }
    private int populationCap;
    public int PopulationCap
    {
        get { return populationCap; }
        set
        {
            SetPopulation(totalPopulation, value);
            populationCap = value;
        }
    }

    public Text pollenText, honeyText, populationText;

    void SetPollen(int count)
    {
        if (honeyText != null)
            pollenText.text = count.ToString();
    }

    public void SetHoney(int count)
    {
        if (honeyText != null)
            honeyText.text = count.ToString();
    }

    public void SetPopulation(int current, int cap)
    {
        if (populationText != null)
            populationText.text = current.ToString() + "/" + cap.ToString();
    }

    public override void Awake()
    {
        base.Awake();

        PopulationCap = 200;
        TotalHoney = 0;
        TotalPollen = 0;
    }
}
