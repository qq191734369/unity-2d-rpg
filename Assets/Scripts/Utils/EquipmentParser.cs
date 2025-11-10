using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentParser
{
    public static Dictionary<string, HumanEquipmentEntity> BuildHumanEquipmemtMap(TextAsset textAsset)
    {
        var res = new Dictionary<string, HumanEquipmentEntity>();

        string[] rows = textAsset.text.Split("\n").Where(d => d.Trim() != "").ToArray();

        for (int i = 0; i < rows.Length; i++)
        {
            if (i != 0)
            {
                string[] cols = rows[i].Split(",").Select(d => d.Trim()).ToArray();
                string id = cols[0];
                string name = cols[1];
                string desc = cols[2];
                HumanEquipmentEntity.Category category = ParseHumanEquipCatepory(cols[3]);
                int level = int.Parse(cols[4]);
                int attack = int.Parse(cols[5]);
                int defense = int.Parse(cols[6]);
                int speed  = int.Parse(cols[7]);

                HumanEquipmentEntity entity = new HumanEquipmentEntity {
                    ID = id,
                    CategoryType = category,
                    EquipmentValues = new BasicValues
                    {
                        Name = name,
                        Description = desc,
                        Attack = attack,
                        Defense = defense,
                        Speed = speed,
                    }
                };

                res.Add(id, entity);
            }
        }

        return res;
    }

    public static HumanEquipmentEntity.Category ParseHumanEquipCatepory(string typeStr)
    {
        switch (typeStr) {
            case "Weapon":
                return HumanEquipmentEntity.Category.Weapon;
            case "Head":
                return HumanEquipmentEntity.Category.Head;
            case "Body":
                return HumanEquipmentEntity.Category.Body;
            case "Shoe":
                return HumanEquipmentEntity.Category.Shoes;
            default:
                return HumanEquipmentEntity.Category.None;
        }
    }
}
