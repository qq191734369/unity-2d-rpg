using UnityEngine;

[CreateAssetMenu(fileName = "BattleBasicInfos", menuName = "Scriptable Objects/BattleBasicInfos")]
public class BattleBasicInfos : ScriptableObject
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
}

