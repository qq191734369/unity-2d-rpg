using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public System.Action OnDataLoadCompleted;

    public System.Action OnDataLoadFailed;

    public System.Action OnDataLoadInProgress;

    [SerializeField]
    public TextAsset chatTextAsset;
    [SerializeField]
    public TextAsset levelExpTextAsset;
    [SerializeField]
    public TextAsset MonsterInfoTextAsset;
    [SerializeField]
    public TextAsset CharacterTextAsset;
    [SerializeField]
    public TextAsset HumanEquipmentTextAsset;
    [SerializeField]
    public TextAsset StoreTextAsset;

    [SerializeField]
    public GameGlobalData gameGlobalData;


    private void Awake()
    {
        gameGlobalData.ChatDictionary = ChatDataParser.BuildChatData(chatTextAsset);
        gameGlobalData.LevelExpMap = LevelDataParser.BuildLevelExpMap(levelExpTextAsset);
        gameGlobalData.MonsterInfoMap = MonsterInfoParser.BuildMonsterInfoMap(MonsterInfoTextAsset);
        CharacterParseResult cRes = CharacterInfoParser.BuildCharacterInfoMap(CharacterTextAsset);
        gameGlobalData.PlayerInfo = cRes.Player;
        gameGlobalData.CharacterInfoMap = cRes.CharacterMap;
        gameGlobalData.HumanEquipmentMap = EquipmentParser.BuildHumanEquipmemtMap(HumanEquipmentTextAsset);
        gameGlobalData.StoreItemMap = StoreParser.ParseStore(StoreTextAsset);
    }

    public ChatSection GetChatSectionByNameAndGroup(string name, string group = "default")
    {
        if (!gameGlobalData.ChatDictionary.ContainsKey(name))
        {
            return null;
        }

        if (!gameGlobalData.ChatDictionary[name].groups.ContainsKey(group))
        {
            return null;
        }

        return gameGlobalData.ChatDictionary[name].groups[group];
    }

    public CharacterEntity GetCharacterByName(string name)
    {
        if (gameGlobalData.CharacterInfoMap.ContainsKey(name))
        {
            return gameGlobalData.CharacterInfoMap[name];
        }
        return null;
    }

    public CharacterEntity GetMonsterInfoByName(string name)
    {
        if (gameGlobalData.MonsterInfoMap.ContainsKey(name))
        {
            return gameGlobalData.MonsterInfoMap[name]?.DeepCopy();
        }
        return null;
    }

    public void AddPartyMember(string name)
    {
        if (gameGlobalData.PartyMemberNameList.Contains(name))
        {
            return;
        }

        gameGlobalData.PartyMemberNameList.Add(name);
    }

    public void RemovePartyMember(string name)
    {
        if (gameGlobalData.PartyMemberNameList.Contains(name))
        {
            gameGlobalData.PartyMemberNameList.Remove(name);
        }
    }

    public List<CharacterEntity> GetPartyMemberEntityList()
    {
        if (gameGlobalData.PartyMemberNameList == null)
        {
            return null;
        }

        return gameGlobalData.PartyMemberNameList.Select((d) => GetCharacterByName(d)).Where(d => d != null).ToList();
    }

    public StoreInfo GetStoreItemsByKey(string key) {
        if (gameGlobalData.StoreItemMap.ContainsKey(key))
        {
            return gameGlobalData.StoreItemMap[key];
        }

        return null;
    }
}

[System.Serializable]
public class ChatSectionByGroup
{
    public Dictionary<string, ChatSection> groups;
}


[System.Serializable]
public class CharacterEntity
{
    public enum ClassType
    {
        None,
        Hunter,
        Warrior
    };

    public enum Race
    {
        None,
        Human,
        Monster,
        Dog
    }

    public string Id;
    public bool IsPlayer;
    public Race race;
    public ClassType classType;
    public BattleBasicInfos info;
    public Vector3 Position;
    public string Scene;
    public CharacterEquipment Equipment;
    public CarEntity CarInfo;
    // 上一次乘坐的车
    public string PreCarId;

    public int Attack
    {
        get
        {
            return info.Attack + Equipment.Attack;
        }
    }

    public int Defense
    {
        get
        {
            return info.Defense + Equipment.Defense;
        }
    }

    public int Speed
    {
        get
        {
            return info.Speed + Equipment.Speed;
        }
    }

    public int MaxHealth
    {
        get
        {
            return info.MaxHealth + Equipment.MaxHealth;
        }
    }

    public int CurrentHealth
    {
        get
        {
            return info.CurrentHealth;
        }
    }


    public CharacterEntity() { }

    public CharacterEntity(CharacterEntity other)
    {
        if (other == null)
        {
            return;
        }

        race = other.race;
        classType = other.classType;
        info = new BattleBasicInfos(other.info);
        Position = new Vector3(other.Position.x, other.Position.y, other.Position.z);
        Scene = other.Scene;
        Equipment = new CharacterEquipment(other.Equipment);
    }

    public CharacterEntity DeepCopy()
    {
        return new CharacterEntity(this);
    }

    public CharacterEntity PreviewInfos(HumanEquipmentEntity equipmentEntity)
    {
        var charactor = DeepCopy();
        var equipCopy = equipmentEntity.DeepCopy();
        switch (equipmentEntity.CategoryType)
        {
            case HumanEquipmentEntity.Category.Weapon:
                charactor.Equipment.Weapon = equipCopy;
                break;
            case HumanEquipmentEntity.Category.Head:
                charactor.Equipment.Head = equipCopy;
                break;
            case HumanEquipmentEntity.Category.Body:
                charactor.Equipment.Body = equipCopy;
                break;
            case HumanEquipmentEntity.Category.Shoes:
                charactor.Equipment.Shoe = equipCopy;
                break;
            case HumanEquipmentEntity.Category.None:
                break;
        }

        return charactor;
    }
}

[System.Serializable]
public class StoreInfo
{
    public enum StoreType
    {
        HumanEquip,
        CarEquip,
        HumanItem,
        CarItem
    }

    public StoreType Type;
    public string Key;
    public string Name;
    public List<string> ItemIds;
}


[System.Serializable]
public class GameGlobalData
{
    // 对话字典
    public Dictionary<string, ChatSectionByGroup> ChatDictionary;
    // 玩家信息
    public CharacterEntity PlayerInfo;
    //// 角色信息
    //public List<CharacterEntity> CharacterEntities;
    // 角色信息
    public Dictionary<string, CharacterEntity> CharacterInfoMap;
    // 队伍列表、为了保证游戏内引用的对象都为globalData便于管理、这里只存名字，在partyManager中进行构建
    public List<string> PartyMemberNameList;
    // 等级-经验表格
    public Dictionary<int, long> LevelExpMap;
    // 怪物信息表
    public Dictionary<string, CharacterEntity> MonsterInfoMap;
    // 人类装备字典
    public Dictionary<string, HumanEquipmentEntity> HumanEquipmentMap;
    // 背包信息
    public BagEntity BagInfo;
    // 商店物品信息
    public Dictionary<string, StoreInfo> StoreItemMap;
}

