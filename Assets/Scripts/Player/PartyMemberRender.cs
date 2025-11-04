using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberRender : MonoBehaviour
{
    private const int PARTY_MEMBER_LAYER = 8;

    private PartyManager partyManager;
    private DataManager dataManager;

    public List<GameObject> MemberVisualList = new List<GameObject>();

    private void Awake()
    {
        GameManager.OnGameInited(init); 
    }

    private void init(GameObject gameManagerObj)
    {
        partyManager = gameManagerObj.GetComponent<PartyManager>();
        dataManager = gameManagerObj.GetComponent<DataManager>();

        // 设置玩家位置
        gameObject.transform.position = dataManager.gameGlobalData.PlayerInfo.Position;

        partyManager.InitedCallback(handlePartyInited);
    }

    private void handlePartyInited(PartyManager p)
    {
        FreshPartyVisualList();
        partyManager.OnPartyMemberChange += FreshPartyVisualList;
    }

    private void FreshPartyVisualList()
    {
        if (MemberVisualList.Count > 0)
        {
            foreach (var item in MemberVisualList)
            {
                Destroy(item);
            }
            MemberVisualList.Clear();
        }

        List<CharacterEntity> characterEntities = partyManager.PartyList;
        for (int i = 0; i < characterEntities.Count; i++) {
            CharacterEntity currentEntity = characterEntities[i];
            Vector3 positionToSpawn = transform.position;
            positionToSpawn.x += 2;
            // 实例化游戏对象
            GameObject tempOverworldMember = Instantiate(currentEntity.info.OverWorldPrefab, positionToSpawn, Quaternion.identity);
            MemberFollowAI memberFollowAI = tempOverworldMember.GetComponent<MemberFollowAI>();
            memberFollowAI.enabled = true;
            memberFollowAI.SetFollowDistance((i + 1));
            tempOverworldMember.layer = PARTY_MEMBER_LAYER;
            tempOverworldMember.SetActive(true);
            tempOverworldMember.GetComponent<Collider2D>().isTrigger = true;
            // 设置基本信息
            tempOverworldMember.GetComponent<CharactorInfo>().CharacterInfo = currentEntity;

            // 读取保存的位置信息
            if (characterEntities[i].Position != null)
            {
                tempOverworldMember.transform.position = currentEntity.Position;
            }

            MemberVisualList.Add(tempOverworldMember);
            Debug.Log($"FreshPartyVisualList, current member {characterEntities[i].info.Name}");
        }
    }
}
