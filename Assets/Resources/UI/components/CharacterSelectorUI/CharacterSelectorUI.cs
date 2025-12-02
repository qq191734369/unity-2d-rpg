using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterSelectorUI : MonoBehaviour,IUIBase
{
    const int SORTING_ORDER = 1001;

    public event System.Action<CharacterEntity> OnSelect;
    public event System.Action<CharacterEntity> OnChange;
    public event System.Action OnDestroy;

    public UIDocument uiDocument;
    public VisualElement templateContainer;
    public VisualElement listContainer;
    public List<CharacterItemUI> itemList = new List<CharacterItemUI>();
    public Label messageLabel;

    public bool isActive = false;

    public int activeIndex = 0;

    public bool isEnabled
    {
        get
        {
            return isActive && UIManager.IsOnTop(this);
        }
    }


    public CharacterSelectorUI() { }

    public static CharacterSelectorUI Create(List<CharacterEntity> memberList, string message = "", string extra = null)
    {
        GameObject gameObject = new GameObject("dynamic ui - CharacterSelectorUI");
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.rotation = Quaternion.identity;

        // 初始化模版
        var uiDocument = gameObject.AddComponent<UIDocument>();
        PanelSettings panelSettings = Resources.Load<PanelSettings>("Settings/UI Toolkit/PanelSettings");
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/components/CharacterSelectorUI/CharacterSelectorUI");
        uiDocument.panelSettings = panelSettings;
        uiDocument.visualTreeAsset = visualTreeAsset;
        uiDocument.sortingOrder = SORTING_ORDER;

        // 获取元素
        var templateContainer = uiDocument.rootVisualElement.Q<VisualElement>("Container");
        var listContainer = templateContainer.Q<VisualElement>("ListContainer");
        Label messageLabel = templateContainer.Q("Message").Q<Label>("Text");
        messageLabel.text = message;
        listContainer.Clear();

        CharacterSelectorUI instance = gameObject.AddComponent<CharacterSelectorUI>();

        List<CharacterItemUI> itemList = new List<CharacterItemUI>();

        instance.uiDocument = uiDocument;
        instance.templateContainer = templateContainer;
        instance.itemList = itemList;
        instance.messageLabel = messageLabel;
        instance.listContainer = listContainer;

        foreach (CharacterEntity member in memberList)
        {
            CharacterItemUI itemUI = new CharacterItemUI(member);
            listContainer.Add(itemUI);
            itemList.Add(itemUI);
        }
        if (extra != null)
        {
            CharacterItemUI itemUI = new CharacterItemUI(extra);
            listContainer.Add(itemUI);
            itemList.Add(itemUI);
        }

        UIManager.Push(instance);
        // 设置激活状态 用于按键判断
        instance.StartInitCorotine();
        Debug.Log("Character select created " + $"is on top {UIManager.IsOnTop(instance)}");

        return instance;
    }

    public void StartInitCorotine()
    {
        StartCoroutine(InitCorotine());
    }

    private IEnumerator InitCorotine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        isActive = true;
        SetActiveBtn(0);
    }

    private IEnumerator CloseCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        UIManager.Pop(this);
        Destroy(gameObject);
    }

    public void Close()
    {
        //StartCoroutine(CloseCoroutine());
        isActive = false;
        UIManager.Pop(this);
        Destroy(gameObject);
        OnDestroy?.Invoke();
    }

    private void SetActiveBtn(int index)
    {
        int targetIndex = index < 0 ? (itemList.Count + index) % itemList.Count : index % itemList.Count;
        itemList.ForEach(d => d.SetActive(false));
        activeIndex = targetIndex;
        itemList[activeIndex].SetActive(true);

        OnChange?.Invoke(itemList[activeIndex].dataCache);
    }

    private void Update()
    {
        if (isEnabled) {
            if (Input.GetKeyUp(KeyCode.Z)) { 
                Close();
            } else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                SetActiveBtn(activeIndex - 1);
            } else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                SetActiveBtn(activeIndex + 1);
            } else if (Input.GetKeyUp(KeyCode.X))
            {
                OnSelect?.Invoke(itemList[activeIndex].dataCache);
            }
        }
    }
}
