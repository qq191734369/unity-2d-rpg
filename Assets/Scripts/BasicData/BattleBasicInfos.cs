using UnityEngine;

//[CreateAssetMenu(fileName = "BattleBasicInfos", menuName = "Scriptable Objects/BattleBasicInfos")]

[System.Serializable]
public class BattleBasicInfos : BasicValues
{
    public readonly static int MAX_LEVEL = LevelManager.MAX_LEVEL;

    public event System.Action<LevelChangeInfo> OnLevelUp;

    public InfoGrowth InfoGrowth;

    public BattleBasicInfos() { }

    public BattleBasicInfos(BattleBasicInfos other)
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

    public LevelChangeInfo SetLevel(int level)
    {
        int deltaLevel = level - Level;
        if (deltaLevel <= 0) {
            return null;
        }
        Level = level;
        return ComputeStatus(deltaLevel);
    }

    private LevelChangeInfo ComputeStatus(int deltaLevel)
    {
        var deltaHealth = InfoGrowth.Health * deltaLevel;
        var deltaAttack = InfoGrowth.Attack * deltaLevel;
        var deltaDefense = InfoGrowth.Defense * deltaLevel;

        MaxHealth = deltaHealth + MaxHealth;
        CurrentHealth = MaxHealth;
        Attack = deltaAttack + Attack;
        Defense = deltaDefense + Defense;

        var levelUp = new LevelChangeInfo
        {
            Attack = deltaAttack,
            Defense = deltaDefense,
            Health = deltaHealth,
            Level = deltaLevel,
            info = this,
        };
        OnLevelUp?.Invoke(levelUp);

        return levelUp;
    }

    public LevelChangeInfo AddLevel(int levelGrowth)
    {
        if (levelGrowth <=0)
        {
            return null;
        }

        int targetLevel = Level + levelGrowth;
        if (targetLevel > BattleBasicInfos.MAX_LEVEL)
        {
            targetLevel = BattleBasicInfos.MAX_LEVEL;
        }

        int deltaLevel = targetLevel - Level;
        if (deltaLevel > 0)
        {
            return ComputeStatus(deltaLevel);
        }

        return null;
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

public class LevelChangeInfo
{
    public BattleBasicInfos info;
    public int Level;
    public int Attack;
    public int Defense;
    public int Health;
    public long NextExp;
}
