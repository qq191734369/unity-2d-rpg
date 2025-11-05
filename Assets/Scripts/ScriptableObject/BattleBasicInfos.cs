using UnityEngine;

//[CreateAssetMenu(fileName = "BattleBasicInfos", menuName = "Scriptable Objects/BattleBasicInfos")]

[System.Serializable]
public class BattleBasicInfos
{
    public static int MAX_LEVEL = 100;

    public int Level;
    public int CurrentExp;
    public string Name;
    public string Description;
    public int MaxHealth;
    public int CurrentHealth;
    public int Speed;
    public int Attack;
    public int Defense;
    public GameObject OverWorldPrefab;
    public GameObject BattlePrefab;

    public InfoGrowth InfoGrowth;

    public BattleBasicInfos() { }

    public BattleBasicInfos(BattleBasicInfos other)
    {
        Level = other.Level;
        CurrentExp = other.CurrentExp;
        Name = other.Name;
        Description = other.Description;
        MaxHealth = other.MaxHealth;
        CurrentHealth = other.CurrentHealth;
        Speed = other.Speed;
        Attack = other.Attack;
        Defense = other.Defense;
        OverWorldPrefab = other.OverWorldPrefab;
        BattlePrefab = other.BattlePrefab;
        InfoGrowth = new InfoGrowth(other.InfoGrowth);
    }

    public bool IsDead
    {
        get {
            return CurrentHealth <= 0;
        }
    }

    public BattleBasicInfos SetLevel(int level)
    {
        int deltaLevel = level - Level;
        if (deltaLevel <= 0) {
            return this;
        }

        ComputeStatus(deltaLevel);

        return this;
    }

    private void ComputeStatus(int deltaLevel)
    {
        MaxHealth = InfoGrowth.Health * deltaLevel + MaxHealth;
        CurrentHealth = MaxHealth;
        Attack = InfoGrowth.Attack * deltaLevel + Attack;
        Defense = InfoGrowth.Defense * deltaLevel + Defense;
    }

    public BattleBasicInfos AddLevel(int levelGrowth)
    {
        if (levelGrowth <=0)
        {
            return this;
        }

        int targetLevel = Level + levelGrowth;
        if (targetLevel > BattleBasicInfos.MAX_LEVEL)
        {
            targetLevel = BattleBasicInfos.MAX_LEVEL;
        }

        int deltaLevel = targetLevel - Level;
        ComputeStatus(deltaLevel);

        return this;
    }
}

public class InfoGrowth
{
    public int Health;
    public int Speed;
    public int Attack;
    public int Defense;

    public InfoGrowth() { }

    public InfoGrowth(InfoGrowth other) {
        Health = other.Health;
        Speed = other.Speed;
        Attack = other.Attack;
        Defense = other.Defense;
    }
}

