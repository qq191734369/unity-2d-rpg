using UnityEngine;


[System.Serializable]
public class CarEquipnent : BasicValues
{
    public enum Type
    {
        // 未定义
        None,
        // C装置
        Core,
        // 主炮
        MainWeapon,
        // 副炮
        SubWeapon,
        // 发动机
        Engine,
        // S-E
        SE,
        // 底盘
        Chassis
    }
    // 物品id
    public string ID;
    // 序列号
    public string No;

    public Type type;

    // 特性 - C装置
    public string C_Skill;

    // 特性 - 底盘
    public string Chassis_Skill;
    // 载重量 - 发动机
    public int LoadCapacity;
    public int MaxLoadCapacity;

    // 单仓 - 底盘、主炮、副炮
    public int WeaponsBay;

    public int MaxAttack;
    public int MaxDefense;
}
