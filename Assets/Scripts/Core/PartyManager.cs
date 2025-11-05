using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void OnPartyManagerInitedCallback(PartyManager obj);

public class PartyManager : MonoBehaviour
{
    // 不含玩家
    [Header("Debug")]
    [SerializeField]
    public List<CharacterEntity> PartyList = new List<CharacterEntity>();

    public System.Action<CharacterEntity> OnJoinedParty;
    public System.Action<CharacterEntity> OnLeftParty;
    public System.Action OnPartyMemberChange;
    public System.Action OnPartyManagerInited;

    public void InitedCallback(OnPartyManagerInitedCallback callback)
    {
        if (dataManager != null) {
            callback(this);
        } else
        {
            OnPartyManagerInited += () => callback(this);
        }
    }

    private DataManager dataManager;

    public List<CharacterEntity> AllMembers
    {
        get
        {
            var list = new List<CharacterEntity>();
            if (dataManager == null) {
                return list;
            }
            var player = dataManager.gameGlobalData.PlayerInfo;
            list.Add(player);
            list.AddRange(PartyList);
            return list;
        }
    }

    private void Awake()
    {
        // init list
        Debug.Log("Party manager awake");
        GameManager.OnGameInited(init);
    }

    private void init(GameObject gameManagerObj) {
        dataManager = gameManagerObj.GetComponent<DataManager>();
        // 根据全局队伍列表数据 构建队伍列表
        List<CharacterEntity> members = dataManager.GetPartyMemberEntityList();
        PartyList.Clear();
        PartyList.AddRange(members);

        OnPartyManagerInited?.Invoke();
    }

    public bool HasJoinedParty(CharacterEntity characterEntity)
    {
        return PartyList.Contains(characterEntity);
    }

    public void JoinParty(CharacterEntity characterEntity) {
        if (HasJoinedParty(characterEntity)) {
            return;
        }
        PartyList.Add(characterEntity);
        dataManager.AddPartyMember(characterEntity.info.Name);

        OnJoinedParty?.Invoke(characterEntity);
        OnPartyMemberChange?.Invoke();
    }

    public void LeaveParty(CharacterEntity characterEntity)
    {
        if (HasJoinedParty(characterEntity))
        {
            PartyList.Remove(characterEntity);
            dataManager.RemovePartyMember(characterEntity.info.Name);
            OnLeftParty?.Invoke(characterEntity);
            OnPartyMemberChange?.Invoke();
        }
    }
}
