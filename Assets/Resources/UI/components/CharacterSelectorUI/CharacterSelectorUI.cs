using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterSelectorUI : MonoBehaviour,IUIBase
{
    const int SORTING_ORDER = 1003;

    public event System.Action<CharacterEntity> OnSelect;

    public UIDocument uiDocument;
    public VisualElement templateContainer;
    public List<CharacterItemUI> itemList = new List<CharacterItemUI>();

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

    public static CharacterSelectorUI Create(List<CharacterEntity> memberList)
    {
        GameObject gameObject = new GameObject("dynamic ui - CharacterSelectorUI");
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.rotation = Quaternion.identity;

        var uiDocument = gameObject.AddComponent<UIDocument>();
        PanelSettings panelSettings = Resources.Load<PanelSettings>("Settings/UI Toolkit/PanelSettings");
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/components/CharacterSelectorUI/CharacterSelectorUI");
        uiDocument.panelSettings = panelSettings;
        uiDocument.visualTreeAsset = visualTreeAsset;
        uiDocument.sortingOrder = SORTING_ORDER;

        var templateContainer = uiDocument.rootVisualElement.Q<VisualElement>("Container");
        templateContainer.Clear();

        CharacterSelectorUI instance = gameObject.AddComponent<CharacterSelectorUI>();

        List<CharacterItemUI> itemList = new List<CharacterItemUI>();

        instance.uiDocument = uiDocument;
        instance.templateContainer = templateContainer;
        instance.itemList = itemList;

        foreach (CharacterEntity member in memberList)
        {
            CharacterItemUI itemUI = new CharacterItemUI(member);
            templateContainer.Add(itemUI);
            itemList.Add(itemUI);
        }
        itemList[0].SetActive(true);

        UIManager.Push(instance);
        // 设置激活状态 用于按键判断
        instance.StartInitCorotine();

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
    }

    public void Close()
    {
        UIManager.Pop(this);
        Destroy(gameObject);
    }

    private void SetActiveBtn(int index)
    {
        int targetIndex = index < 0 ? (itemList.Count + index) % itemList.Count : index % itemList.Count;
        itemList.ForEach(d => d.SetActive(false));
        activeIndex = targetIndex;
        itemList[activeIndex].SetActive(true);
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
