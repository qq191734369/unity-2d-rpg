using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public UIDocument uiDocument;
    private Button exitGameBtn;
    private GameManager gameManager;
    private PartyManager partyManager;

    private void Awake()
    {
        GameManager.OnGameInited(InitPage);
    }

    void InitPage(GameObject gameManagerObj)
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        partyManager = gameManagerObj.GetComponent<PartyManager>();

        exitGameBtn = uiDocument.rootVisualElement.Q<Button>("ExitButton");
        exitGameBtn.clicked += ExitGame;

        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
  
    }

    void ExitGame()
    {
        Debug.Log("clicked exit game btn");
        GameActions.ExitGame();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C)) {
            CreateCharacterListUI();
        }
    }

    private void CreateCharacterListUI()
    {
        uiDocument.rootVisualElement.style.display = uiDocument.rootVisualElement.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        VisualElement pannel = uiDocument.rootVisualElement.Q<VisualElement>("CharacterPannel");
        if (pannel == null) {
            return;
        }

        var allPartyMembers = partyManager.AllMembers;
        if (allPartyMembers == null) {
            return;
        }

        pannel.Clear();
        foreach (var member in allPartyMembers) {
            var characterCard = new CharacterCard(member);
            pannel.Add(characterCard);
        }
    }
}
