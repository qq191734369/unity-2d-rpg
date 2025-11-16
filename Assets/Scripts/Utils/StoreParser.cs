using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreParser
{
    public static Dictionary<string, StoreInfo> ParseStore(TextAsset textAsset)
    {
        var res = new Dictionary<string, StoreInfo>();

        string[] rows = textAsset.text.Split("\n").Where(d => d.Trim() != "").ToArray();

        for (int i = 0; i < rows.Length; i++)
        {
            if (i != 0)
            {
                string[] cols = rows[i].Trim().Split(',').Select(d => d.Trim()).ToArray();
                string key = cols[0];
                StoreInfo.StoreType type = ParseStoreType(cols[1]);
                List<string> itemIds = cols[2].Split(";").Where(d => d != null && d != "").Select(d => d.Trim()).ToList();

                res.Add(key, new StoreInfo {
                    Key = key,
                    //Name = key,
                    Type = type,
                    ItemIds = itemIds
                });
            }
        }


        return res;
    }

    private static StoreInfo.StoreType ParseStoreType(string value) {
        switch (value) {
            case "HumanEquip":
                return  StoreInfo.StoreType.HumanEquip;
            case "HumanItem":
                return StoreInfo.StoreType.HumanItem;
            case "CarEquip":
                return StoreInfo.StoreType.CarEquip;
            case "CarItem":
                return StoreInfo.StoreType.CarItem;
            default:
                return StoreInfo.StoreType.HumanEquip;
        }
    }
}
