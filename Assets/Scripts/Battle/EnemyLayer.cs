using System.Collections.Generic;
using UnityEngine;

public class EnemyLayer : MonoBehaviour
{
    [SerializeField]
    List<SceneEnemyInfo> Enemies = new List<SceneEnemyInfo>();
    [SerializeField, Range(1, 8)]
    int MinEnemyCount = 1;
    [SerializeField, Range(1, 8)]
    int MaxEnemyCount = 8;

    public void StartBattle()
    {
        var enemyList = GenerateEnemies();
        GameManager.SavePosiiton();
        SceneLoader.LoadBattleScene(new SceneParams
        {
            EnemyList = enemyList,
        });
    }

    private List<SceneEnemyInfo> GenerateEnemies()
    {
        var res = new List<SceneEnemyInfo>();
        int count = Random.Range(MinEnemyCount, MaxEnemyCount + 1);

        for (int i = 1; i <= count; i++)
        {
            int enemyIndex = Random.Range(0, Enemies.Count);
            var info = Enemies[enemyIndex];
            res.Add(new SceneEnemyInfo
            {
                Name = info.Name,
                Level = info.Level,
            });
        }

        return res;
    } 
}
