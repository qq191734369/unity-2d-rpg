using UnityEngine;

public class ResourceUtil
{
    public static T GetCharacterAvartar<T>(CharacterEntity entity) where T : Object
    {
        var race = entity.race;
        string name = entity.info.Name;
        string id = entity.Id;
        string folderName;
        switch (race)
        {
            case CharacterEntity.Race.Monster:
                folderName = "Monsters";
                break;
            default:
                folderName = "Characters";
                break;
        }


        // 优先找头像
        var avartar1 = Resources.Load<T>($"Sprites/{folderName}/{id}/{id}-Avatar");
        if (avartar1 != null)
        {
            return avartar1;
        }

        var fallback = Resources.Load<T>($"Sprites/{folderName}/{id}/{id}");

        return fallback;
    }

    public static T GetCharacterAvartar<T>(string id) where T : Object
    {
        // 优先找头像
        var avartar1 = Resources.Load<T>($"Sprites/Characters/{id}/{id}-Avatar");
        if (avartar1 != null)
        {
            return avartar1;
        }

        var fallback1 = Resources.Load<T>($"Sprites/Characters/{id}/{id}");
        if (fallback1 != null)
        {
            return fallback1;
        }

        var avartar2 = Resources.Load<T>($"Sprites/Monsters/{id}/{id}-Avatar");
        if (avartar2 != null)
        {
            return avartar2;
        }

        var fallback2 = Resources.Load<T>($"Sprites/Monsters/{id}/{id}");
        if (fallback2 != null)
        {
            return fallback2;
        }

        return null;
    }
}
