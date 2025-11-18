using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreUI : MonoBehaviour,IUIBase
{

    private UIDocument uIDocument;
    private ScrollView scrollView;
    private VisualElement listContent;
    private HumanEquipmentSystem humanEquipmentSystem;
    private DataManager dataManager;
    private PartyManager partyManager;
    private BagManager bagManager;

    private List<HumanEquipmentEntity> humanEquipmentEntities = new List<HumanEquipmentEntity>();
    private List<StoreItemComponent> storeItems = new List<StoreItemComponent>();
    private VisualElement detailContainerElm;
    private VisualElement characterInfoElm;
    private Label goldValueElm;

    private int activeSelectedIndex = -1;

    private bool isActive = false;

    public bool IsEnable
    {
        get { return isActive && UIManager.IsOnTop(this); }
    }


    private void Awake()
    {
        uIDocument = GetComponent<UIDocument>();
        scrollView = uIDocument.rootVisualElement.Q<ScrollView>("ItemList");
        listContent = scrollView.Q<VisualElement>("ListContent");

        uIDocument.rootVisualElement.style.display = DisplayStyle.None;
        detailContainerElm = uIDocument.rootVisualElement.Q<VisualElement>("DetailContainer");
        characterInfoElm = uIDocument.rootVisualElement.Q<VisualElement>("CharacterInfo");
        goldValueElm = uIDocument.rootVisualElement.Q<Label>("GoldValue");

        GameManager.OnGameInited(Init);
    }

    private void Init(GameObject gameManagerObj) {
        humanEquipmentSystem = gameManagerObj.GetComponent<HumanEquipmentSystem>();
        dataManager = gameManagerObj.GetComponent<DataManager>();
        partyManager = gameManagerObj.GetComponent<PartyManager>();
        bagManager = gameManagerObj.GetComponent<BagManager>();
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
    }

    private void RefreshGoldValue()
    {
        goldValueElm.text = bagManager.Gold.ToString();
    }

    private void GenerateItems(string key)
    {
        var storeInfo = dataManager.GetStoreItemsByKey(key);
        StoreInfo.StoreType type = storeInfo.Type;
        List<string> ids = storeInfo.ItemIds;

        humanEquipmentEntities.Clear();
        foreach (string id in ids) {
            humanEquipmentEntities.Add(humanEquipmentSystem.GetHumanEquipmentById(id));
        }
    }

    private void RenderItems()
    {
        storeItems.Clear();
        listContent.Clear();

        humanEquipmentEntities.ForEach(d =>
        {
            var id = d.ID;
            var equipmentEntity = humanEquipmentSystem.GetHumanEquipmentById(id);
            var item = new StoreItemComponent(equipmentEntity);
            storeItems.Add(item);
            listContent.Add(item);
        });

        SetActive(0);
    }

    public void Show(string storeKey)
    {
        if (UIManager.Contains(this)) {
            return;
        }

        // 商店打开后清除所有其他UI
        //UIManager.ClearAll();
        Debug.Log("Storee UI Show");

        UIManager.Push(this);
        StartCoroutine(InitCoroutine());
        uIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        detailContainerElm.Clear();
        activeSelectedIndex = -1;
        GenerateItems(storeKey);
        RenderItems();
        RefreshGoldValue();
    }

    public void Close()
    {
        UIManager.Pop(this);
        StartCoroutine(CloseCoroutine());
        uIDocument.rootVisualElement.style.display = DisplayStyle.None;
        activeSelectedIndex = -1;
        storeItems.Clear();
        listContent.Clear();
    }

    private void SetActive(int index)
    {
        if (storeItems == null || storeItems.Count == 0)
        {
            return;
        } 

        int length = storeItems.Count;
        int target = index < 0 ? Math.Abs(length + index) % length : index % length;
        storeItems.ForEach(d => d.SetAcitve(false));
        activeSelectedIndex = target;
        storeItems[activeSelectedIndex].SetAcitve(true);

        scrollView.ScrollTo(storeItems[activeSelectedIndex]);

        detailContainerElm.Clear();
        detailContainerElm.Add(new HumanEquipmentInfoCard(storeItems[activeSelectedIndex].HumanEquipmentCache));
    }

    private void HandleConfirmPress()
    {
        if (activeSelectedIndex == -1)
        {
            return;
        }
        Debug.Log("Store UI Press X");
        var selectedEquipId = storeItems[activeSelectedIndex].HumanEquipmentCache.ID;
        var instance = CharacterSelectorUI.Create(partyManager.AllMembers, "给谁装备？", "背包");
        instance.OnSelect += (CharacterEntity entity) =>
        {
            var newEquip = humanEquipmentSystem.GetHumanEquipmentById(selectedEquipId);
            if (entity == null)
            {
                if (bagManager.HasEnoughGold(newEquip.Price))
                {
                    bagManager.DecreaseGold(newEquip.Price);
                    bagManager.AddItem(newEquip);
                    RefreshGoldValue();
                    instance.Close();
                }
                
            }
            else
            {
                if (bagManager.HasEnoughGold(newEquip.Price))
                {
                    bagManager.DecreaseGold(newEquip.Price);
                    humanEquipmentSystem.Equip(entity, newEquip);
                    RefreshGoldValue();
                    instance.Close();
                }
            }
        };
        instance.OnChange += (CharacterEntity entity) =>
        {
            characterInfoElm.Clear();
            if (activeSelectedIndex == -1 || entity == null)
            {
                return;
            }
            var equipment = storeItems[activeSelectedIndex]?.HumanEquipmentCache;
            characterInfoElm.Add(new CommonCharacterInfoCardComponent(entity, equipment));
        };
        instance.OnDestroy += () =>
        {
            characterInfoElm.Clear();
        };
    }

    private void DetectUserPress()
    {
        if (!IsEnable)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            SetActive(activeSelectedIndex - 1);
        } else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            SetActive(activeSelectedIndex + 1);
        } else if (Input.GetKeyUp(KeyCode.X))
        {
            HandleConfirmPress();
        } else if (Input.GetKeyUp(KeyCode.Z))
        {
            Close();
        }
    }

    private void Update()
    {
        DetectUserPress();
    }
}
