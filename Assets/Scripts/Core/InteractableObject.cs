using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // 对象名称，用于查找对话配置
    [SerializeField] string Name;
    // 对话分组
    [SerializeField] string Group;

    [Header("Debug")]
    // 对话链表
    [SerializeField]
    public ChatSection chatSection;

    public CharacterEntity CharacterInfo;

    private DataManager dataManager;
    private PartyManager partyManager;

    private void Awake()
    {
        GameManager.OnGameInited(init);
    }

    private void init(GameObject gameManagerObj)
    {
        dataManager = gameManagerObj.GetComponent<DataManager>();
        partyManager = gameManagerObj.GetComponent<PartyManager>();

        CharacterInfo = dataManager.GetCharacterByName(Name);

        if (partyManager.HasJoinedParty(CharacterInfo))
        {
            gameObject.SetActive(false);
        }
    }

    public ChatSection GetCurrentChatSection()
    {
        if (Name == null)
        {
            return null;
        }

        if (Group == null)
        {
            return null;
        }

        chatSection = dataManager.GetChatSectionByNameAndGroup(Name, Group);
        return chatSection;
    }
}
