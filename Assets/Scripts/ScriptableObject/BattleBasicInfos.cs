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

    public bool IsDead
    {
        get {
            return CurrentHealth <= 0;
        }
    }
}
