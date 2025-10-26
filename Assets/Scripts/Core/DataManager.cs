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
    public GameGlobalData gameGlobalData;


    private void Awake()
    {
        gameGlobalData.ChatDictionary = ChatDataParser.BuildChatData(chatTextAsset);
        gameGlobalData.LevelExpMap = LevelDataParser.BuildLevelExpMap(levelExpTextAsset);
        gameGlobalData.MonsterInfoMap = MonsterInfoParser.BuildMonsterInfoMap(MonsterInfoTextAsset);
    }

    public ChatSection GetChatSectionByNameAndGroup(string name, string group = "default")
    {
        return gameGlobalData.ChatDictionary[name].groups[group];
    }

    public CharacterEntity GetCharacterByName(string name)
    {
        return gameGlobalData.CharacterEntities.Find((d) => d.info.Name == name);
    }

    public CharacterEntity GetMonsterInfoByName(string name) {
        return gameGlobalData.MonsterInfoMap[name]?.DeepCopy();
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
        Human,
        Monster,
        Dog
    }

    public Race race;
    public ClassType classType;
    public BattleBasicInfos info;

    public CharacterEntity DeepCopy()
    {
        string json = JsonUtility.ToJson(this);
        return JsonUtility.FromJson<CharacterEntity>(json);
    }
}

[System.Serializable]
public class GameGlobalData
{
    // 对话字典
    public Dictionary<string, ChatSectionByGroup> ChatDictionary;
    // 玩家信息
    public CharacterEntity PlayerInfo;
    // 角色信息
    public List<CharacterEntity> CharacterEntities;
    // 等级-经验表格
    public Dictionary<int, int> LevelExpMap;
    // 怪物信息表
    public Dictionary<string, CharacterEntity> MonsterInfoMap;
}

