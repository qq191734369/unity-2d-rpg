using UnityEngine;

public class CarSystem : MonoBehaviour
{
    public static void Equip(CarEntity carEntity, CarEquipnent carEquipnent, CarEquipmentInfos.SlotPosition slotPosition)
    {
        // 战车装备
        var type = carEquipnent.type;
        var currentEquipments = carEntity.EquipmentInfo;

        var slot = GetSlot(slotPosition, currentEquipments);

        if (slot == null)
        {
            return;
        }

        var slotType = slot.type;
        if (slotType != carEquipnent.type)
        {
            return;
        }

        // 包满了
        var currentEquipStorageSize = carEntity.EquipnentStorage.Count;
        if (currentEquipStorageSize >= CarEntity.MAX_EQUIP_NUM && slot.Equipnent != null)
        {
            return;
        }

        var currentEquipment = slot.Equipnent;
        slot.Equipnent = carEquipnent;
        if (currentEquipment != null) {
            // 放入战车背包
            carEntity.EquipnentStorage.Add(currentEquipment);
        }
    }

    /// <summary>
    /// 向战车添加物品
    /// </summary>
    /// <returns>Boolean</returns>
    public static bool AddToCar(CarEntity car, CarEquipnent equipnent)
    {
        if (car.EquipnentStorage.Count >= CarEntity.MAX_EQUIP_NUM)
        {
            return false;
        }

        car.EquipnentStorage.Add(equipnent);
        return true;
    }

    public static bool AddToCar(CarEntity car, CarItem item)
    {
        if (car.ItemStorage.Count >= CarEntity.MAX_ITEM_NUM)
        {
            return false;
        }

        car.ItemStorage.Add(item);
        return true;
    }

    public static void Boarding(CarEntity car, CharacterEntity character)
    {
        var drivers = car.Drivers;
        if (drivers.Contains(character))
        {
            return;
        }

        if (drivers.Count >= CarEntity.MAX_ITEM_NUM)
        {
            return;
        }

        drivers.Add(character);
        character.CarInfo = car;
        character.PreCarId = car.Id;
    }

    public static void Alighting(CarEntity car, CharacterEntity character)
    {
        var drivers = car.Drivers;
        if (drivers.Contains(character))
        {
            drivers.Remove(character);
            character.CarInfo = null;
        }
    }


    private static CarEquipSlot GetSlot(CarEquipmentInfos.SlotPosition slotPosition, CarEquipmentInfos carEquipmentInfos)
    {
        switch (slotPosition)
        {
            case CarEquipmentInfos.SlotPosition.Chassis:
                return carEquipmentInfos.Chassis;
            case CarEquipmentInfos.SlotPosition.Core1:
                return carEquipmentInfos.Core1;
            case CarEquipmentInfos.SlotPosition.Core2:
                return carEquipmentInfos.Core2;
            case CarEquipmentInfos.SlotPosition.Engine1:
                return carEquipmentInfos.Engine1;
            case CarEquipmentInfos.SlotPosition.Engine2:
                return carEquipmentInfos.Engine2;
            case CarEquipmentInfos.SlotPosition.Weapon1:
                return carEquipmentInfos.Weapon1;
            case CarEquipmentInfos.SlotPosition.Weapon2:
                return carEquipmentInfos.Weapon2;
            case CarEquipmentInfos.SlotPosition.Weapon3:
                return carEquipmentInfos.Weapon3;
            case CarEquipmentInfos.SlotPosition.Weapon4:
                return carEquipmentInfos.Weapon4;
            default:
                return null;
        }
    }
}
