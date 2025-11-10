using UnityEngine;

[System.Serializable]
public class BasicValues
{
    public int Level;
    public long DealthExp;
    public long CurrentExp;
    public string Name;
    public string Description;
    public int MaxHealth;
    public int CurrentHealth;
    public int Speed;
    public int Attack;
    public int Defense;
    public GameObject OverWorldPrefab;
    public GameObject BattlePrefab;

    public BasicValues() { }

    public BasicValues(BasicValues other)
    {
        Level = other.Level;
        DealthExp = other.DealthExp;
        CurrentExp = other.CurrentExp;
        Name = other.Name;
        Description = other.Description;
        MaxHealth = other.MaxHealth;
        CurrentHealth = other.CurrentHealth;
        Speed = other.Speed;
        Attack = other.Attack;
        Defense = other.Defense;
    }
}
