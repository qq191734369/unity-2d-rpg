using UnityEngine;

[System.Serializable]
public class CharacterEquipment
{
    public HumanEquipmentEntity Weapon;
    public HumanEquipmentEntity Head;
    public HumanEquipmentEntity Body;
    public HumanEquipmentEntity Shoe;

    public CharacterEquipment() { }

    public CharacterEquipment(CharacterEquipment other)
    {
        if (other == null)
        {
            return;
        }
        Weapon = other.Weapon;
        Head = other.Head;
        Body = other.Body;
        Shoe = other.Shoe;
    }

    public int Attack
    {
        get
        {
            return 
                (Weapon?.EquipmentValues?.Attack ?? 0)
                + (Head?.EquipmentValues?.Attack ?? 0)
                + (Body?.EquipmentValues?.Attack ?? 0)
                + (Shoe?.EquipmentValues?.Attack ?? 0);
        }
    }

    public int Defense
    {
        get
        {
            return
                (Weapon?.EquipmentValues?.Defense ?? 0)
                + (Head?.EquipmentValues?.Defense ?? 0)
                + (Body?.EquipmentValues?.Defense ?? 0)
                + (Shoe?.EquipmentValues?.Defense ?? 0);
        }
    }

    public int MaxHealth
    {
        get
        {
            return
                (Weapon?.EquipmentValues?.MaxHealth ?? 0)
                + (Head?.EquipmentValues?.MaxHealth ?? 0)
                + (Body?.EquipmentValues?.MaxHealth ?? 0)
                + (Shoe?.EquipmentValues?.MaxHealth ?? 0);
        }
    }

    public int Speed
    {
        get
        {
            return
                (Weapon?.EquipmentValues?.Speed ?? 0)
                + (Head?.EquipmentValues?.Speed ?? 0)
                + (Body?.EquipmentValues?.Speed ?? 0)
                + (Shoe?.EquipmentValues?.Speed ?? 0);
        }
    }

}
