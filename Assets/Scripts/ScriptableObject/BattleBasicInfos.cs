using UnityEngine;

//[CreateAssetMenu(fileName = "BattleBasicInfos", menuName = "Scriptable Objects/BattleBasicInfos")]

[System.Serializable]
public class BattleBasicInfos
{
    public int Level;
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
        this.Level = other.Level;
        this.Name = other.Name;
        this.Description = other.Description;
        this.MaxHealth = other.MaxHealth;
        this.CurrentHealth = other.CurrentHealth;
        this.Speed = other.Speed;
        this.Attack = other.Attack;
        this.Defense = other.Defense;
        this.OverWorldPrefab = other.OverWorldPrefab;
        this.BattlePrefab = other.BattlePrefab;
        this.InfoGrowth = new InfoGrowth(other.InfoGrowth);
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

        MaxHealth = InfoGrowth.Health * deltaLevel + MaxHealth;
        CurrentHealth = MaxHealth;
        Attack = InfoGrowth.Attack * deltaLevel + Attack;
        Defense = InfoGrowth.Defense * deltaLevel + Defense;

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

