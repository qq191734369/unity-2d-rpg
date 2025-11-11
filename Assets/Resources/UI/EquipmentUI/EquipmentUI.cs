using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EquipmentUI : MonoBehaviour
{
    private BagManager bagManager;
    private HumanEquipmentSystem humanEquipmentSystem;

    private VisualElement root;

    private bool isActive = false;
    public bool isEnabled
    {
        get { return isActive && UIManager.IsOnTop(this); }
    }

    // 角色立绘
    private VisualElement characterPicSlot;
    // 角色信息
    private VisualElement characterInfoSlot;
    // 当前装备
    private VisualElement currentEquipListWrapperElm;
    private List<EquipListItemComponent> equipListItemComponentList = new List<EquipListItemComponent>();
    // 背包装备
    private VisualElement bagEquipListWrapperElm;
    private List<EquipListItemComponent> bagListItemComponentList = new List<EquipListItemComponent>();

    // 当前装备信息
    private VisualElement equiptedInfoElm;
    // 背包装备信息
    private VisualElement selectedInfoElm;

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
        characterPicSlot = root.Q<VisualElement>("Pic");
        characterInfoSlot = root.Q<VisualElement>("CharacterInfoBanner");
        equiptedInfoElm = root.Q<VisualElement>("EquiptedInfo");
        selectedInfoElm = root.Q<VisualElement>("SelectedInfo");

        GameManager.OnGameInited(HandleGameInited);
    }

    private void HandleGameInited(GameObject gameManagerObject) {
        bagManager = gameManagerObject.GetComponent<BagManager>();
        humanEquipmentSystem = gameManagerObject.GetComponent<HumanEquipmentSystem>();
    }

    private void RenderCurrentEquipments()
    {
        ClearBagItemInfo();
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
            ClearEquipedItemInfo();
            return;
        }
        int targetIndex = idx < 0 ? Math.Abs(length + idx) % length : idx % length;
        equipListItemComponentList?.ForEach(d => d.SetActive(false));

        currentEquipIndex = targetIndex;
        equipListItemComponentList[targetIndex].SetActive(true);
        RenderEquipedItemInfo();
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
        isActive = false;
        UIManager.Pop(this);
    }

    private void RenderPic()
    {
        var info = character.info;
        characterPicSlot.style.backgroundImage = Resources.Load<Texture2D>($"Sprites/Characters/{info.Name}/{info.Name}-BigPic");
    }

    private void RenderCharacterInfo()
    {
        CharacterInfoCardComponent comp = new CharacterInfoCardComponent(character);
        characterInfoSlot.Clear();
        characterInfoSlot.Add(comp);
    }

    public void Show(CharacterEntity characterEntity)
    {
        UIManager.Push(this);
        character = characterEntity;

        RenderPic();
        RenderCharacterInfo();


        RenderCurrentEquipments();
        RenderBagList();
        root.style.display = DisplayStyle.Flex;

        StartCoroutine(InitCoroutine());
    }

    private void ResetState() {
        activePanel = ActivePannel.Equipted;
        currentBagIndex = 0;
        currentEquipIndex = 0;
    }

    public void Close()
    {
        root.style.display = DisplayStyle.None;
        ResetState();
        StartCoroutine(CloseCoroutine());
    }

    private void RenderBagItemInfo()
    {
        var currentBagEquip = bagListItemComponentList[currentBagIndex].dataCache;
        VisualElement content = selectedInfoElm.Q("Content");
        selectedInfoElm.style.opacity = 0;
        content.Clear();
        if (currentBagEquip == null)
        {
            return;
        }

        selectedInfoElm.style.opacity = 1;
        content.Add(new EquipmentInfoContainerComponent(currentBagEquip));
    }

    private void RenderEquipedItemInfo()
    {
        var currentEquip = equipListItemComponentList[currentEquipIndex].dataCache;
        VisualElement content = equiptedInfoElm.Q("Content");
        content.Clear();
        equiptedInfoElm.style.opacity = 0;
        if (currentEquip == null)
        {
            return;
        }

        equiptedInfoElm.style.opacity = 1;
        content.Add(new EquipmentInfoContainerComponent(currentEquip));
    }

    private void ClearBagItemInfo()
    {
        selectedInfoElm.style.opacity = 0;
        VisualElement content = selectedInfoElm.Q("Content");
        content.Clear();
    }

    private void ClearEquipedItemInfo()
    {
        equiptedInfoElm.style.opacity = 0;
        VisualElement content = equiptedInfoElm.Q("Content");
        content.Clear();
    }


    private void RenderBagList()
    {
        bagEquipListWrapperElm.Clear();
        bagListItemComponentList.Clear();
        
        if (activePanel != ActivePannel.Bag)
        {
            return;
        }

        var currentType = equipListItemComponentList[currentEquipIndex].HumanEquipCategory;
        var items = bagManager.GetHumanEquipmentListByCategory(currentType);

        // 如果道具数量为0 则返回装备栏
        if (items == null || items.Count == 0)
        {
            activePanel = ActivePannel.Equipted;
            return;
        }

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
            ClearBagItemInfo();
            return;
        }
        int targetIndex = idx < 0 ? Math.Abs(length + idx) % length : idx % length;
        bagListItemComponentList?.ForEach(d => d.SetActive(false));

        currentBagIndex = targetIndex;
        bagListItemComponentList[targetIndex].SetActive(true);

        RenderBagItemInfo();
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
            Equip();
        }
    }

    private void Equip()
    {
        var selectedBagEquipment = bagListItemComponentList[currentBagIndex];
        if (selectedBagEquipment != null) {
            humanEquipmentSystem.Equip(character, selectedBagEquipment.dataCache);

            // 返回装备栏
            activePanel = ActivePannel.Equipted;
            RenderCurrentEquipments();
            RenderBagList();

            // 刷新显示
            RenderCharacterInfo();
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
