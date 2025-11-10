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
        Weapon = other.Weapon;
        Head = other.Head;
        Body = other.Body;
        Shoe = other.Shoe;
    }
}
