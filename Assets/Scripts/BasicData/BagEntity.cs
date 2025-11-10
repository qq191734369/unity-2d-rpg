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
    // 随机生成的编号
    public string No;

    public string ID;
    public enum Category
    {
        None,
        Weapon,
        Head,
        Body,
        Shoes
    }

    public Category CategoryType;
    // 装备基本属性
    public BasicValues EquipmentValues;

    // 哪位角色装备
    //public CharacterEntity Character;

    public HumanEquipmentEntity() { }

    public HumanEquipmentEntity(HumanEquipmentEntity other)
    {
        ID = other.ID;
        CategoryType = other.CategoryType;
        EquipmentValues = new BasicValues(other.EquipmentValues);
    }

    public HumanEquipmentEntity DeepCopy()
    {
        var res = new HumanEquipmentEntity(this);
        
        return res;
    }
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
