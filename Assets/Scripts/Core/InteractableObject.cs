using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // �������ƣ����ڲ��ҶԻ�����
    [SerializeField] string Name;
    // �Ի�����
    [SerializeField] string Group;

    [Header("Debug")]
    // �Ի�����
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
