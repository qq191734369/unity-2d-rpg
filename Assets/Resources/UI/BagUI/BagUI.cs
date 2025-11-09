using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BagUI : MonoBehaviour, IUIBase
{
    const string ACTIVE_CLASS = "active";

    const int HUMAN_ITEM_CATEGORY_INDEX = 4;

    private UIDocument uIDocument;

    private bool isActive = false;

    private BagManager bagManager;

    private VisualElement equipListContent;
    private VisualElement detailContainer;

    private List<EquipmentItem> equipmentItemUIElementList;
    private int activeListIndex = 0;

    private List<Button> typeBtnList;
    private int activeTypeIndex = 0;

    private List<Button> categoryBtnList;
    private int activeCategoryIndex = 0;
    private VisualElement humanSubTypeSelectorElm;
    private VisualElement carSubTypeSelectorElm;


    private enum ActionStackName
    {
        SelectingType,
        //SelectingCategory,
        SelectingEquipment,
    }

    private Stack<ActionStackName> actionStack = new Stack<ActionStackName>();

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
        ClearList();

        typeBtnList = uIDocument.rootVisualElement.Q("TypeSelector").Query<Button>(classes: "typeSelectorItem").ToList();
        humanSubTypeSelectorElm = uIDocument.rootVisualElement.Q<VisualElement>("HumanSubTypeSelector");
        carSubTypeSelectorElm = uIDocument.rootVisualElement.Q<VisualElement>("CarSubTypeSelector");

        // 默认为选择类型
        PushStack(ActionStackName.SelectingType);
        SetActiveTypeBtn(0);

        GameManager.OnGameInited(Init);
    }

    private void Init(GameObject gameManagerObj)
    {
        bagManager = gameManagerObj.GetComponent<BagManager>();
    }

    private void PushStack(ActionStackName name)
    {
        if (actionStack.Count == 0)
        {
            actionStack.Push(name);
            return;
        }

        if (actionStack.Peek() == name || actionStack.Contains(name))
        {
            return;
        }

        actionStack.Push(name);
    }

    private void PopStack()
    {
        if (actionStack.Count == 1)
        {
            return;
        }


        actionStack.Pop();

    }

    private bool isCurrentStatus(ActionStackName name)
    {
        if (actionStack.Count == 0)
        {
            return false;
        }
        return actionStack.Peek() == name;
    }

    public void Show()
    {
        StartCoroutine(ShowCorotine());
    }

    private IEnumerator ShowCorotine()
    {
        UIManager.Push(this);

        uIDocument.rootVisualElement.style.display = DisplayStyle.Flex;

        activeCategoryIndex = 0;
        RenderCategoryByType();

        //PushStack(ActionStackName.SelectingEquipment);
        //RenderListByCategory();
        yield return new WaitForSecondsRealtime(0.1f);

        isActive = true;
        Debug.Log($"Bag UI avtive {isActive}");

        Debug.Log($"Show Bag UI, stack peek {actionStack.Peek()}");
        yield return null;
    }

    public void Close()
    {
        uIDocument.rootVisualElement.style.display = DisplayStyle.None;
        isActive = false;
        UIManager.Pop(this);
    }

    private void RenderCategoryByType()
    {
        if (isCurrentStatus(ActionStackName.SelectingType))
        {
            categoryBtnList?.Clear();
            humanSubTypeSelectorElm.style.display = DisplayStyle.None;
            carSubTypeSelectorElm.style.display = DisplayStyle.None;

            return;
        }

        if (activeTypeIndex == 0)
        {
            categoryBtnList?.Clear();
            humanSubTypeSelectorElm.style.display = DisplayStyle.Flex;
            carSubTypeSelectorElm.style.display= DisplayStyle.None;

            categoryBtnList = humanSubTypeSelectorElm.Query<Button>().ToList();
            SetActiveCategoryBtn(0);
        }
        else
        {
            categoryBtnList?.Clear();
            humanSubTypeSelectorElm.style.display = DisplayStyle.None;
            carSubTypeSelectorElm.style.display = DisplayStyle.Flex;

            categoryBtnList = carSubTypeSelectorElm.Query<Button>().ToList();
            SetActiveCategoryBtn(0);

        }
        Debug.Log($"Render Category List, stack peek {actionStack.Peek()}");
    }

    private void SetActiveCategoryBtn(int index)
    {
        if (categoryBtnList == null)
        {
            return;
        }

        int targetIndex = index < 0 ? Math.Abs(categoryBtnList.Count + index) : index;

        categoryBtnList.ForEach(d => d.RemoveFromClassList(ACTIVE_CLASS));
        activeCategoryIndex = targetIndex % categoryBtnList.Count;
        categoryBtnList[activeCategoryIndex].AddToClassList(ACTIVE_CLASS);
    }

    private void ClearList()
    {
        equipListContent.Clear();
        detailContainer.Clear();
    }

    private void RenderListByCategory()
    {
        if (isCurrentStatus(ActionStackName.SelectingType))
        {
            ClearList();
            return;
        }
        // 人类道具
        if (activeTypeIndex == 0) {
            if (activeCategoryIndex == HUMAN_ITEM_CATEGORY_INDEX)
            {
                RenderHumanItemList();
            }
            else
            {
                RenderHunanEquipmentList();
            }
        } else
        {

        }

        Debug.Log($"Render List By Category, stack peek {actionStack.Peek()}, index: {activeCategoryIndex}");
    }

    private void ShowCurrentDetail()
    {
        detailContainer.Clear();
        if (equipmentItemUIElementList == null || equipmentItemUIElementList.Count == 0) { return; }
        var currentItem = equipmentItemUIElementList[activeListIndex];
        var element = new HumanEquipmentInfoCard(currentItem.dataCache);
        detailContainer.Add(element);
    }

    // 展示当前装备列表
    private void RenderHunanEquipmentList()
    {
        var bagData = bagManager.GetBagData();
        var humanEquipments = bagData.HumanEquipmentList;

        equipListContent.Clear();
        detailContainer.Clear();

        if (humanEquipments == null)
        {
            return;
        }

        var currentType = HumanEquipmentEntity.Category.Weapon;
        switch (activeCategoryIndex)
        {
            case 0:
                currentType = HumanEquipmentEntity.Category.Weapon;
                break;
            case 1:
                currentType = HumanEquipmentEntity.Category.Head;
                break;
            case 2:
                currentType = HumanEquipmentEntity.Category.Body;
                break;
            case 3:
                currentType = HumanEquipmentEntity.Category.Shoes;
                break;
            default:
                break;
        }

        var currentEquips = humanEquipments.Where(d => d.CategoryType == currentType).ToList();
        if (currentEquips.Count == 0)
        {
            return;
        }

        List<EquipmentItem> itemList = new List<EquipmentItem>();
        foreach (var item in currentEquips)
        {
            var equipItem = new EquipmentItem(item);
            equipListContent.Add(equipItem);
            itemList.Add(equipItem);
        }

        equipmentItemUIElementList = itemList;
        activeListIndex = 0;
        itemList[activeListIndex].SetActive(true);
        ShowCurrentDetail();
    }

    private void RenderHumanItemList()
    {
        equipListContent.Clear();
        detailContainer.Clear();
    }

    private void SetActiveItem(KeyCode keyCode)
    {
        if (!isCurrentStatus(ActionStackName.SelectingEquipment))
        {
            return;
        }

        if (equipmentItemUIElementList == null || equipmentItemUIElementList.Count == 0)
        {
            return;
        }

        int targetIndex = 0;
        switch (keyCode)
        {
            case KeyCode.DownArrow:
                targetIndex = (activeListIndex + 1) % equipmentItemUIElementList.Count;
                break;
            case KeyCode.UpArrow:
                targetIndex = Math.Max(activeListIndex - 1, 0) % equipmentItemUIElementList.Count;
                break;
            default:
                return;
        }

        var pre = equipmentItemUIElementList[activeListIndex];
        pre.SetActive(false);
        activeListIndex = targetIndex;
        equipmentItemUIElementList[activeListIndex].SetActive(true);

        ShowCurrentDetail();
    }

    private void SetActiveTypeBtn(int index)
    {
        int targetIndex = index < 0 ? Math.Abs(typeBtnList.Count + index) : index;
        typeBtnList.ForEach(d => d.RemoveFromClassList(ACTIVE_CLASS));
        activeTypeIndex = targetIndex % typeBtnList.Count;
        typeBtnList[activeTypeIndex].AddToClassList(ACTIVE_CLASS);
    }


    private void DetectUserPress()
    {
        if (!isEnabled)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (isCurrentStatus(ActionStackName.SelectingEquipment))
            {
                PopStack();
                RenderCategoryByType();
                RenderListByCategory();
            }
            else
            {
                Close();
            }
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            SetActiveItem(KeyCode.DownArrow);
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            SetActiveItem(KeyCode.UpArrow);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            // 选择类型
            if (isCurrentStatus(ActionStackName.SelectingType))
            {
                SetActiveTypeBtn(activeTypeIndex - 1);
            }
            else if (isCurrentStatus(ActionStackName.SelectingEquipment))
            // 选择装备种类
            {
                if (categoryBtnList == null)
                {
                    return;
                }

                SetActiveCategoryBtn(activeCategoryIndex - 1);
                RenderListByCategory();
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            // 选择类型
            if (isCurrentStatus(ActionStackName.SelectingType))
            {
                SetActiveTypeBtn(activeTypeIndex + 1);
            }
            else if (isCurrentStatus(ActionStackName.SelectingEquipment))
            {
                if (categoryBtnList == null)
                {
                    return;
                }

                SetActiveCategoryBtn(activeCategoryIndex + 1);
                RenderListByCategory();
            }
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            if (isCurrentStatus(ActionStackName.SelectingType))
            {
                PushStack(ActionStackName.SelectingEquipment);
                RenderCategoryByType();
                RenderListByCategory();
                Debug.Log($"Bag UI X Press, action stack peek {actionStack.Peek()}");
            }
        }
    }

    private void Update()
    {
        DetectUserPress();
    }
}
