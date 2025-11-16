using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour, IUIBase
{
    const string ACTION_BTN_ACTIVE_CLASS = "active";

    [SerializeField]
    UIDocument bagUI;
    [SerializeField]
    UIDocument equipmentUI;

    private UIDocument uiDocument;

    private Button exitGameBtn;

    private bool isActive = false;

    private PartyManager partManager;

    private VisualElement actionContainer;

    private int activeActionBtnIndex;
    private List<Button> actionButtons;

    private bool isEnabled
    {
        get
        {
            return isActive && UIManager.IsOnTop(this);
        }
    }

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        exitGameBtn = uiDocument.rootVisualElement.Q<Button>("ExitButton");
        exitGameBtn.clicked += ExitGame;

        Close();

        GameManager.OnGameInited(InitPage);
    }

    void InitPage(GameObject gameManagerObj)
    {
        partManager = gameManagerObj.GetComponent<PartyManager>();
        actionContainer = uiDocument.rootVisualElement.Q<VisualElement>("Actions");
    }

    void ExitGame()
    {
        Debug.Log("clicked exit game btn");
        GameActions.ExitGame();
    }

    public void Show()
    {
        UIManager.Push(this);
        // 人物列表渲染
        CreateCharacterListUI(partManager.AllMembers);
        isActive = true;
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        InitMenuBtns();
    }

    public void Close()
    {
        isActive = false;
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        activeActionBtnIndex = 0;
        UIManager.Pop(this);
    }

    private void CreateCharacterListUI(List<CharacterEntity> allMembers)
    {
        VisualElement pannel = uiDocument.rootVisualElement.Q<VisualElement>("CharacterPannel");
        if (pannel == null)
        {
            return;
        }

        var allPartyMembers = allMembers;
        if (allPartyMembers == null)
        {
            return;
        }

        pannel.Clear();
        foreach (var member in allPartyMembers)
        {
            var characterCard = new CharacterCard(member);
            pannel.Add(characterCard);
        }
    }

    private void InitMenuBtns()
    {
        if (actionContainer == null)
        {
            return;
        }
        actionButtons = actionContainer.Query<Button>(classes: "actionBtn").ToList();
        if (actionButtons.Count > 0)
        {
            actionButtons.ForEach(d => d.RemoveFromClassList(ACTION_BTN_ACTIVE_CLASS));
            activeActionBtnIndex = 0;
            actionButtons[activeActionBtnIndex].AddToClassList(ACTION_BTN_ACTIVE_CLASS);
        }
    }

    private void DetectBtnPress()
    {
        if (!isEnabled)
        {
            return;
        }

        int btnLength = actionButtons.Count;

        int targetIndex = 0;
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            targetIndex = (activeActionBtnIndex + 1) % btnLength;
            SetActiveBtn(targetIndex);
        } else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            targetIndex = Math.Abs((activeActionBtnIndex - 1)) % btnLength;
            SetActiveBtn(targetIndex);
        } else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            targetIndex = (activeActionBtnIndex + 2) % btnLength;
            SetActiveBtn(targetIndex);
        } else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            targetIndex = Math.Abs((activeActionBtnIndex - 2)) % btnLength;
            SetActiveBtn(targetIndex);
        } else if (Input.GetKeyUp(KeyCode.X))
        {
            HandleActionPress();
        } else if (Input.GetKeyUp(KeyCode.Z))
        {
            HandleCancelBtnPress();
        }
    }

    private void HandleActionPress()
    {
        var activeBtn = actionButtons[activeActionBtnIndex];
        string name = activeBtn.name;
        switch (name)
        {
            case "BagBtn":
                bagUI.GetComponent<BagUI>().Show();
                break;
            case "EquipBtn":
                HandleEquipBtnPress();
                break;
            default:
                break;
        }
    }

    private void HandleEquipBtnPress()
    {
        var members = partManager.AllMembers;
        var characterSelector = CharacterSelectorUI.Create(members);

        characterSelector.OnSelect += (CharacterEntity entity) =>
        {
            Debug.Log($"Equip ui Select character {entity.info.Name}");
            EquipmentUI eq = equipmentUI.GetComponent<EquipmentUI>();
            eq.Show(entity);
        };
    }

    private void HandleCancelBtnPress()
    {
        Close();
    }

    private void SetActiveBtn(int targetIndex)
    {
        var preBtn = actionButtons[activeActionBtnIndex];
        preBtn.RemoveFromClassList(ACTION_BTN_ACTIVE_CLASS);
        activeActionBtnIndex = targetIndex;
        actionButtons[activeActionBtnIndex].AddToClassList(ACTION_BTN_ACTIVE_CLASS);
        Debug.Log($"Active Index {activeActionBtnIndex}");
    }

    private void Update()
    {
        DetectBtnPress();
    }
}
