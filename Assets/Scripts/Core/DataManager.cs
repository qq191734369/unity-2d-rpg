using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    public TextAsset textAsset;

    public System.Action OnDataLoadCompleted;

    public System.Action OnDataLoadFailed;

    public System.Action OnDataLoadInProgress;

    [SerializeField]
    public GameGlobalData gameGlobalData;


    private void Awake()
    {
        gameGlobalData.ChatDictionary = ChatDataParser.BuildChatData(textAsset);
    }

    public ChatSection GetChatSectionByNameAndGroup(string name, string group = "default")
    {
        return gameGlobalData.ChatDictionary[name].groups[group];
    }

    public CharacterEntity GetCharacterByName(string name)
    {
        return gameGlobalData.CharacterEntities.Find((d) => d.info.Name == name);
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
}

