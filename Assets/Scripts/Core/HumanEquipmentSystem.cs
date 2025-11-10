using System.Collections.Generic;
using UnityEngine;

public class HumanEquipmentSystem : MonoBehaviour
{
    private DataManager dataManager;
    private BagManager bagManager;

    private Dictionary<string, HumanEquipmentEntity> humanEquipmentMap = new Dictionary<string, HumanEquipmentEntity>();

    private void Awake()
    {
        GameManager.OnGameInited(Init);
    }

    private void Init(GameObject gameManagerObj)
    {
        dataManager = gameManagerObj.GetComponent<DataManager>();
        humanEquipmentMap = dataManager.gameGlobalData?.HumanEquipmentMap;
    }

    public HumanEquipmentEntity GetHumanEquipmentById(string id)
    {
        if (!humanEquipmentMap.ContainsKey(id))
        {
            return null;
        }

        var res = dataManager.gameGlobalData.HumanEquipmentMap[id]?.DeepCopy();
        res.No = NoGenerator.GenerateRandomDigits();

        return res;
    }

    private void AddToBag(HumanEquipmentEntity humanEquipmentEntity)
    {
        if (humanEquipmentEntity == null)
        {
            return;
        }

        bagManager.AddItem(humanEquipmentEntity);
    }

    public HumanEquipmentSystem Equip(CharacterEntity charater, HumanEquipmentEntity equipment)
    {
        switch (equipment.CategoryType) {
            case HumanEquipmentEntity.Category.Weapon:
                var weapon = charater.Equipment.Weapon;
                AddToBag(weapon);
                charater.Equipment.Weapon = equipment;
                break;
            case HumanEquipmentEntity.Category.Head:
                var head = charater.Equipment.Head;
                AddToBag(head);
                charater.Equipment.Head = equipment;
                break;
            case HumanEquipmentEntity.Category.Body:
                var body = charater.Equipment.Head;
                AddToBag(body);
                charater.Equipment.Body = equipment;
                break;
            case HumanEquipmentEntity.Category.Shoes:
                var shoe = charater.Equipment.Shoe;
                AddToBag(shoe);
                charater.Equipment.Shoe = equipment;
                break;
            default:
                break;
        }

        return this;
    }
}
