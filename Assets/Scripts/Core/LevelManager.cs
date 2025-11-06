using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public readonly static int MAX_LEVEL = 100;

    static DataManager dataManager;
    static Dictionary<int, long> expMap;

    private void Awake()
    {
        GameManager.OnGameInited(HandleGameInit);
    }

    private void HandleGameInit(GameObject gameManagerObj)
    {
        dataManager = gameManagerObj.GetComponent<DataManager>();
        expMap = dataManager.gameGlobalData.LevelExpMap;
    }

    public static LevelChangeInfo AddExp(CharacterEntity characterEntity, long exp) {
        if (expMap == null) {
            return null;
        }

        int maxLevel = GetMaxLevel();


        if (exp < 0)
        {
            return null;
        }

        var info = characterEntity.info;
        long maxExp = expMap[maxLevel];
        long currentExp = info.CurrentExp;
        long currentLevel = info.Level;
        // ¼ÆËãÄ¿±êexp
        long targetExp = currentExp + exp;
        targetExp = targetExp >= maxExp ? maxExp : targetExp;
        info.CurrentExp = targetExp;

        int targetLevel = ComputeLevel(targetExp);

        if (targetLevel <= currentLevel) {
            return null;
        }

        var levelUpInfo = info.SetLevel(targetLevel);

        long expToNext = GetExpToLevelUp(characterEntity);
        levelUpInfo.NextExp = expToNext;

        return levelUpInfo;
    }

    public static long GetExpToLevelUp(CharacterEntity characterEntity)
    {
        int level = characterEntity.info.Level;
        int maxLevel = GetMaxLevel();
        if (level == maxLevel)
        {
            return 0;
        }

        long nextLevelExp = expMap[level + 1];

        return expMap[level + 1] - characterEntity.info.CurrentExp;
    }

    private static int GetMaxLevel()
    {
        int maxLevel = Math.Min(MAX_LEVEL, expMap.Keys.ToList()[expMap.Count - 1]);
        return maxLevel;
    }


    private static int ComputeLevel(long targetExp)
    {
        var expMap = dataManager.gameGlobalData.LevelExpMap;
        foreach (var k in expMap.Keys)
        {
            if (targetExp < expMap[k])
            {
                return k - 1;
            }
        }

        return GetMaxLevel();
    }
}
