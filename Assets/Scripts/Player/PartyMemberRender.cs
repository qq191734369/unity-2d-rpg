using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberRender : MonoBehaviour
{
    private const int PARTY_MEMBER_LAYER = 8;

    private PartyManager partyManager;

    private List<GameObject> memberVisualList = new List<GameObject>();

    private void Awake()
    {
        GameManager.OnGameInited(init); 
    }

    private void init(GameObject gameManagerObj)
    {
        partyManager = gameManagerObj.GetComponent<PartyManager>();

        FreshPartyVisualList();
        partyManager.OnPartyMemberChange += FreshPartyVisualList;
    }

    private void FreshPartyVisualList()
    {
        if (memberVisualList.Count > 0)
        {
            foreach (var item in memberVisualList)
            {
                Destroy(item);
            }
            memberVisualList.Clear();
        }

        List<CharacterEntity> characterEntities = partyManager.PartyList;
        for (int i = 0; i < characterEntities.Count; i++) {
            Vector3 positionToSpawn = transform.position;
            positionToSpawn.x += 2;
            GameObject tempOverworldMember = Instantiate(characterEntities[i].info.OverWorldPrefab, positionToSpawn, Quaternion.identity);
            MemberFollowAI memberFollowAI = tempOverworldMember.GetComponent<MemberFollowAI>();
            memberFollowAI.enabled = true;
            memberFollowAI.SetFollowDistance(2*(i + 1));
            tempOverworldMember.layer = PARTY_MEMBER_LAYER;
            tempOverworldMember.SetActive(true);
            tempOverworldMember.GetComponent<Collider2D>().isTrigger = true;
            memberVisualList.Add(tempOverworldMember);
            Debug.Log($"FreshPartyVisualList, current member {characterEntities[i].info.Name}");
        }
    }
}
