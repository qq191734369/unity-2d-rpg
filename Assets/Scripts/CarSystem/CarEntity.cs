using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;


// 战车基础属性
[System.Serializable]
public class CarInfos : BasicValues
{

}

public class CarEquipSlot
{
    public CarEquipnent.Type type;
    // 是否已经开启
    public bool IsOpen;
    public CarEquipnent Equipnent;
}

[System.Serializable]
public class CarEquipmentInfos
{
    // 槽位
    public enum SlotPosition {
        None,
        Chassis,
        Core1,
        Core2,
        Engine1,
        Engine2,
        Weapon1,
        Weapon2,
        Weapon3,
        Weapon4,
    };

    // 底盘
    public CarEquipSlot Chassis = new CarEquipSlot {
        type = CarEquipnent.Type.Chassis,
        IsOpen = true,
        Equipnent = null
    };
    // 核心
    public CarEquipSlot Core1 = new CarEquipSlot
    {
        type = CarEquipnent.Type.Core,
        IsOpen = true,
        Equipnent = null
    };
    public CarEquipSlot Core2 = new CarEquipSlot
    {
        type = CarEquipnent.Type.Core,
        IsOpen = false,
        Equipnent = null
    };
    // 引擎
    public CarEquipSlot Engine1 = new CarEquipSlot
    {
        type = CarEquipnent.Type.Engine,
        IsOpen = true,
        Equipnent = null
    };
    public CarEquipSlot Engine2 = new CarEquipSlot
    {
        type = CarEquipnent.Type.Engine,
        IsOpen = false,
        Equipnent = null
    };
    // 武器
    public CarEquipSlot Weapon1 = new CarEquipSlot
    {
        type = CarEquipnent.Type.MainWeapon,
        IsOpen = true,
        Equipnent = null
    };
    public CarEquipSlot Weapon2 = new CarEquipSlot
    {
        type = CarEquipnent.Type.SubWeapon,
        IsOpen = true,
        Equipnent = null
    };
    public CarEquipSlot Weapon3 = new CarEquipSlot
    {
        type = CarEquipnent.Type.None,
        IsOpen = false,
        Equipnent = null
    };
    public CarEquipSlot Weapon4 = new CarEquipSlot
    {
        type = CarEquipnent.Type.None,
        IsOpen = false,
        Equipnent = null
    };
}

[System.Serializable]
public class CarItem
{
    public string ID;
    public string Name;
    public string Description;
    public int Count;
}


// 战车实体
[System.Serializable]
public class CarEntity
{
    public static readonly int MAX_EQUIP_NUM = 8;
    public static readonly int MAX_ITEM_NUM = 16;
    public static int MAX_DRIVER_COUNT = 2;

    public string Id;
    public string Name;
    public string Description;
    public CarInfos Infos = new CarInfos();
    public CarEquipmentInfos EquipmentInfo = new CarEquipmentInfos();
    public string Scene;
    public Vector2 Position;

    // 车仓库装备列表
    public List<CarEquipnent> EquipnentStorage = new List<CarEquipnent>();
    // 车载道具列表
    public List<CarItem> ItemStorage = new List<CarItem>();

    // 驾驶员列表
    public List<CharacterEntity> Drivers = new List<CharacterEntity>();
}
