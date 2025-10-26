using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterInfoParser
{
    public static Dictionary<string, CharacterEntity> BuildMonsterInfoMap(TextAsset textAsset)
    {
        Dictionary<string, CharacterEntity> keyValuePairs = new Dictionary<string, CharacterEntity>();

        string[] rows = textAsset.text.Split("\n").Where(d => d.Trim() != "").ToArray();

        for (int i = 0; i < rows.Length; i++)
        {
            if (i != 0)
            {
                string[] cols = rows[i].Trim().Split(',').Select(d => d.Trim()).ToArray();
                string id = cols[0];
                string name = cols[1];
                int exp = int.Parse(cols[2]);
                int baseHealth = int.Parse(cols[3]);
                int baseDefense = int.Parse(cols[4]);
                int baseAttack = int.Parse(cols[5]);
                int baseSpeed = int.Parse(cols[6]);

                int heatlGrowth = int.Parse(cols[7]);
                int defenseGrowth = int.Parse(cols[8]);
                int attackGrowth = int.Parse(cols[9]);
                int speedGrowth = 0;
                string prefabName = cols[11];
                string desc = cols[12];

                var info = new BattleBasicInfos
                {
                    Level = 1,
                    Name = name,
                    Description = desc,
                    MaxHealth = baseHealth,
                    CurrentHealth = baseHealth,
                    Speed = baseSpeed,
                    Attack = baseAttack,
                    Defense = baseDefense,
                    BattlePrefab = Resources.Load<GameObject>($"Prefabs/Enemy/{id}-battle")
                };

                info.InfoGrowth = new InfoGrowth
                {
                    Attack = attackGrowth,
                    Defense = defenseGrowth,
                    Health = heatlGrowth,
                    Speed = 0
                };

                var charactorInfo = new CharacterEntity();
                charactorInfo.info = info;

                keyValuePairs.Add(id, charactorInfo);
            }
        }

        return keyValuePairs;
    }
}
