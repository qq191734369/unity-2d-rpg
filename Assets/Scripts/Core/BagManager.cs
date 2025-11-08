using System;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    public static readonly int MAX_ITEM_NUM = 99;

    private DataManager dataManager;

    private BagEntity bagEntity;

    private void Awake()
    {
        GameManager.OnGameInited(OnGameInited);
    }

    private void OnGameInited(GameObject gameManagerObj) { 
        dataManager = gameManagerObj.GetComponent<DataManager>();
        bagEntity = dataManager.gameGlobalData.BagInfo;
    }

    private HumanItemEntity GetCurrentItem(HumanItemEntity item)
    {
        return bagEntity.HumanItemList.Find(d => d.ItemId == item.ItemId);
    }

    public BagEntity GetBagData()
    {
        return dataManager.gameGlobalData.BagInfo;
    }

    // 添加物品
    public BagManager AddItem(HumanItemEntity item)
    {
        var existItem = GetCurrentItem(item);
        if (existItem == null)
        {
            bagEntity.HumanItemList.Add(item);
            return this;
        }

        if (existItem.CategoryType == HumanItemEntity.Category.Reusable)
        {
            return this;
        }

        int targetCount = existItem.Count + item.Count;
        existItem.Count = Math.Min(targetCount, MAX_ITEM_NUM);
        return this;
    }

    public BagManager AddItem(HumanEquipmentEntity item)
    {
        bagEntity.HumanEquipmentList.Add(item);
        return this;
    }

    // 移除物品
    public BagManager RemoveItem(HumanItemEntity item)
    {
        var existItem = GetCurrentItem(item);
        if (existItem == null)
        {
            return this;
        }

        if (existItem.CategoryType == HumanItemEntity.Category.Reusable)
        {
            bagEntity.HumanItemList.Remove(item);
            return this;
        }

        if (existItem.Count > 0)
        {
            existItem.Count--;
        }

        if (existItem.Count <= 0)
        {
            bagEntity.HumanItemList.Remove(item);
        }
        return this;
    }

    public BagManager RemoveItem(HumanEquipmentEntity item)
    {
        bagEntity.HumanEquipmentList.Remove(item);
        return this;
    }
}
