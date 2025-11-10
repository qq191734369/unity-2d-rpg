using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentUI : MonoBehaviour
{
    private BagManager bagManager;

    private VisualElement root;

    private bool isActive = false;
    public bool isEnabled
    {
        get { return isActive && UIManager.IsOnTop(this); }
    }

    // 当前装备
    private VisualElement currentEquipListWrapperElm;
    private List<EquipListItemComponent> equipListItemComponentList = new List<EquipListItemComponent>();
    // 背包装备
    private VisualElement bagEquipListWrapperElm;
    private List<EquipListItemComponent> bagListItemComponentList = new List<EquipListItemComponent>();

    private enum ActivePannel
    {
        Equipted,
        Bag
    }

    private CharacterEntity character;

    private ActivePannel activePanel = ActivePannel.Equipted;
    private int currentEquipIndex = 0;
    private int currentBagIndex = 0;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;

        currentEquipListWrapperElm = root.Q<VisualElement>("CurrentEquipment").Q<VisualElement>("ItemWrapper");
        bagEquipListWrapperElm = root.Q<VisualElement>("EquipmentInBag").Q<VisualElement>("BagEquipListWrapper");

        GameManager.OnGameInited(HandleGameInited);
    }

    private void HandleGameInited(GameObject gameManagerObject) {
        bagManager = gameManagerObject.GetComponent<BagManager>();
    }

    private void RenderCurrentEquipments()
    {
        var characterEquipment = character.Equipment;
        if (characterEquipment == null) {
            return;
        }

        var weapon = characterEquipment.Weapon;
        var head = characterEquipment.Head;
        var body = characterEquipment.Body;
        var shoe = characterEquipment.Shoe;

        currentEquipListWrapperElm.Clear();
        equipListItemComponentList.Clear();
        AddEquipToContainer(weapon, "武器:", HumanEquipmentEntity.Category.Weapon);
        AddEquipToContainer(head, "头部:", HumanEquipmentEntity.Category.Head);
        AddEquipToContainer(body, "盔甲:", HumanEquipmentEntity.Category.Body);
        AddEquipToContainer(shoe, "鞋子:", HumanEquipmentEntity.Category.Shoes);

        SetActiveCurrentItem(currentEquipIndex);
    }

    private void SetActiveCurrentItem(int idx)
    {
        int length = equipListItemComponentList.Count;
        if (length == 0)
        {
            return;
        }
        int targetIndex = idx < 0 ? Math.Abs(length + idx) % length : idx % length;
        equipListItemComponentList?.ForEach(d => d.SetActive(false));

        currentEquipIndex = targetIndex;
        equipListItemComponentList[targetIndex].SetActive(true);
    }

    private void AddEquipToContainer(HumanEquipmentEntity item, string tag, HumanEquipmentEntity.Category category)
    {
        var elm = new EquipListItemComponent(item, tag, category);
        currentEquipListWrapperElm.Add(elm);
        equipListItemComponentList.Add(elm);
    }

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        isActive = true;
    }

    private IEnumerator CloseCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        UIManager.Pop(this);
    }

    public void Show(CharacterEntity characterEntity)
    {
        UIManager.Push(this);
        character = characterEntity;
        RenderCurrentEquipments();
        RenderBagList();
        root.style.display = DisplayStyle.Flex;

        StartCoroutine(InitCoroutine());
    }

    public void Close()
    {
        root.style.display = DisplayStyle.None;
        StartCoroutine(CloseCoroutine());
    }


    private void RenderBagList()
    {
        bagEquipListWrapperElm.Clear();
        bagListItemComponentList.Clear();
        if (activePanel != ActivePannel.Bag)
        {
            return;
        }

        var currentType = equipListItemComponentList[currentEquipIndex].category;
        var items = bagManager.GetHumanEquipmentListByCategory(currentType);
        foreach (var item in items) {
            var elm = new EquipListItemComponent(item);
            bagEquipListWrapperElm.Add(elm);
            bagListItemComponentList.Add(elm);
        }

        if (items?.Count > 0)
        {
            SetActiveBagItem(0);
        }
    }

    private void SetActiveBagItem(int idx)
    {
        int length = bagListItemComponentList.Count;
        if (length == 0)
        {
            return;
        }
        int targetIndex = idx < 0 ? Math.Abs(length + idx) % length : idx % length;
        bagListItemComponentList?.ForEach(d => d.SetActive(false));

        currentBagIndex = targetIndex;
        bagListItemComponentList[targetIndex].SetActive(true);
    }

    private void HandleConfirmPress()
    {
        if (activePanel == ActivePannel.Equipted)
        {
            activePanel = ActivePannel.Bag;
            RenderBagList();
        }
        else if (activePanel == ActivePannel.Bag)
        {
             
        }
    }


    private void HandleCancelPress()
    {
        if (activePanel == ActivePannel.Equipted)
        {
            Close();
        } else
        {
            activePanel = ActivePannel.Equipted;
            RenderCurrentEquipments();
            RenderBagList();
        }
    }

    private void HandleUpPress()
    {
        if (activePanel == ActivePannel.Equipted)
        {
            SetActiveCurrentItem(currentEquipIndex - 1);
        } else if (activePanel == ActivePannel.Bag)
        {
            SetActiveBagItem(currentBagIndex - 1);
        }
    }

    private void HandleDownPress()
    {
        if (activePanel == ActivePannel.Equipted)
        {
            SetActiveCurrentItem(currentEquipIndex + 1);
        }
        else if (activePanel == ActivePannel.Bag)
        {
            SetActiveBagItem(currentBagIndex + 1);
        }
    }

    private void DetectUserPress()
    {
        if (!isEnabled)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            HandleConfirmPress();
        } else if (Input.GetKeyUp(KeyCode.Z))
        {
           HandleCancelPress();
        } else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            HandleUpPress();
        } else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            HandleDownPress();
        }
    }

    private void Update()
    {
        DetectUserPress();
    }
}
