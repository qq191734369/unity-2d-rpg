using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class BagEntity
{
    public List<HumanEquipmentEntity> HumanEquipmentList = new List<HumanEquipmentEntity>();
    public List<HumanItemEntity> HumanItemList = new List<HumanItemEntity>();
}


[System.Serializable]
public class HumanEquipmentEntity
{
    public string ID;
    public enum Category
    {
        Weapon,
        Head,
        Body,
        Shoes
    }

    public Category CategoryType;
    // 装备基本属性
    public BasicValues EquipmentValues;
}

[System.Serializable]
public class HumanItemEntity
{
    public string ItemId;
    public enum Category {
        Consumables,
        Reusable
    }
    public Category CategoryType;
    public string Name;
    public string Desc;
    public int Count;
}
