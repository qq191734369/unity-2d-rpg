using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BagUI : MonoBehaviour, IUIBase
{
    const string ACTIVE_CLASS = "active";

    private UIDocument uIDocument;

    private bool isActive = false;

    private BagManager bagManager;

    private VisualElement equipListContent;
    private VisualElement detailContainer;

    private List<EquipmentItem> equipmentItemUIElementList;
    private int activeIndex = 0;
    private bool isEnabled
    {
        get
        {
            return isActive && UIManager.IsOnTop(this);
        }
    }

    private void Awake()
    {
        uIDocument = GetComponent<UIDocument>();
        uIDocument.rootVisualElement.style.display = DisplayStyle.None;
        equipListContent = uIDocument.rootVisualElement.Q<VisualElement>("EquipListContent");
        detailContainer = uIDocument.rootVisualElement.Q<VisualElement>("DetailContainer");

        GameManager.OnGameInited(Init);
    }

    private void Init(GameObject gameManagerObj)
    {
        bagManager = gameManagerObj.GetComponent<BagManager>();
    }

    public void Show() {
        uIDocument.rootVisualElement.style.display = DisplayStyle.Flex;

        RenderEquipmentList();

        isActive = true;

        UIManager.Push(this);
    }

    public void Close() {
        uIDocument.rootVisualElement.style.display = DisplayStyle.None;
        isActive = false;
        UIManager.Pop(this);
    }

    private void ShowCurrentDetail()
    {
        detailContainer.Clear();
        if (equipmentItemUIElementList == null || equipmentItemUIElementList.Count == 0) { return; }
        var currentItem = equipmentItemUIElementList[activeIndex];
        var element = new HumanEquipmentInfoCard(currentItem.dataCache);
        detailContainer.Add(element);
    }

    private void RenderEquipmentList()
    {
        var bagData = bagManager.GetBagData();
        var humanEquipments = bagData.HumanEquipmentList;
        equipListContent.Clear();
        detailContainer.Clear();

        List<EquipmentItem> itemList = new List<EquipmentItem>();
        foreach (var item in humanEquipments) {
            var equipItem = new EquipmentItem(item);
            equipListContent.Add(equipItem);
            itemList.Add(equipItem);
        }

        equipmentItemUIElementList = itemList;
        activeIndex = 0;
        itemList[activeIndex].SetActive(true);
        ShowCurrentDetail();
    }

    private void SetActiveItem(KeyCode keyCode)
    {
        if (equipmentItemUIElementList == null || equipmentItemUIElementList.Count == 0)
        {
            return;
        }

        int targetIndex = 0;
        switch (keyCode)
        {
            case KeyCode.DownArrow:
                targetIndex = (activeIndex + 1) % equipmentItemUIElementList.Count;
                break;
            case KeyCode.UpArrow:
                targetIndex = Math.Abs((activeIndex - 1)) % equipmentItemUIElementList.Count;
                break;
            default:
                return;
        }

        var pre = equipmentItemUIElementList[activeIndex];
        pre.SetActive(false);
        activeIndex = targetIndex;
        equipmentItemUIElementList[activeIndex].SetActive(true);

        ShowCurrentDetail();
    }


    private void DetectUserPress()
    {
        if (isEnabled) {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                Close();
            } else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                SetActiveItem(KeyCode.DownArrow);
            } else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                SetActiveItem(KeyCode.UpArrow);
            }
        }
    }

    private void Update()
    {
        DetectUserPress();
    }
}
