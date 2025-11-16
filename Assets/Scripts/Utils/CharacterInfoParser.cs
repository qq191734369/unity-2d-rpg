using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterParseResult
{
    public CharacterEntity Player;
    public Dictionary<string, CharacterEntity> CharacterMap;
}


public class CharacterInfoParser
{
    public static CharacterParseResult BuildCharacterInfoMap(TextAsset textAsset)
    {
        var res = new Dictionary<string, CharacterEntity>();
        CharacterEntity playerInfo = null;

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
                CharacterEntity.ClassType classType = GetClassInfo(cols[13]);
                bool isPlayer = cols[14] == "Y" ? true : false;
                CharacterEntity.Race race = GetRace(cols[15]);


                var info = new BattleBasicInfos
                {
                    Level = 1,
                    Name = name,
                    Description = desc,
                    CurrentExp = exp,
                    MaxHealth = baseHealth,
                    CurrentHealth = baseHealth,
                    Speed = baseSpeed,
                    Attack = baseAttack,
                    Defense = baseDefense,
                    OverWorldPrefab = Resources.Load<GameObject>($"Prefabs/Character/{id}/{id}"),
                    BattlePrefab = Resources.Load<GameObject>($"Prefabs/Character/{id}/{id}-battle")
                };

                var equipment = new CharacterEquipment { };

                info.InfoGrowth = new InfoGrowth
                {
                    Attack = attackGrowth,
                    Defense = defenseGrowth,
                    Health = heatlGrowth,
                    Speed = 0
                };

                var charactorInfo = new CharacterEntity {
                    Id = id,
                    info = info,
                    classType = classType,
                    race = race,
                    IsPlayer = isPlayer,
                    Equipment = equipment,
                };

                res.Add(id, charactorInfo);
                if (isPlayer)
                {
                    playerInfo = charactorInfo;
                }
            }
        }

        return new CharacterParseResult {
            Player = playerInfo,
            CharacterMap = res
        };
    }

    private static CharacterEntity.ClassType GetClassInfo(string className)
    {
        switch (className) {
            case "Hunter":
                return CharacterEntity.ClassType.Hunter;
            case "Warrior":
                return CharacterEntity.ClassType.Warrior;
            default:
                return CharacterEntity.ClassType.None;
        }
    }

    private static CharacterEntity.Race GetRace(string raceName) {
        switch (raceName) {
            case "Human":
                return CharacterEntity.Race.Human;
            case "Dog":
                return CharacterEntity.Race.Dog;
            case "Monster":
                return CharacterEntity.Race.Monster;
            default:
                return CharacterEntity.Race.None;
        }
    }
}
