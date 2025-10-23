using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    const string PARTY_MEMBER_KEY = "PartyMember";
    const string GAME_MANAGER_KEY = "GameManager";
    const string ENEMY_MEMBER_KEY = "Enemy";

    [SerializeField]
    public List<CharacterEntity> enemies = new List<CharacterEntity>();
    [SerializeField]
    public UIDocument battleUIDocument;
    [SerializeField, Range(0, 100)]
    public int RunChance;
    [SerializeField]
    public bool isDebug;

    public enum BattleActionStatus
    {
        // ѡ����Ϊ
        SelectAction,
        // ѡ�����
        SelectEnemy,
        // �ڻغ���
        InBattle,
        // ʤ��
        Win,
        // ʧ��
        Fail
    }

    public static BattleActionStatus CurrentBattleStatus = BattleActionStatus.SelectAction;

    private PartyManager partyManager;
    private List<CharacterEntity> partyMembers;
    [SerializeField]
    private List<BattleVisual> enemyBattleVisualList = new List<BattleVisual>();
    [SerializeField]
    private List<BattleVisual> charactorBattleVisualList = new List<BattleVisual>();

    private BattleUI battleUI;

    private int currentBattleCharacterIndex = 0;

    private List<BattleVisual> battleQueue = new List<BattleVisual>();

    // �����ظ���ʼ��
    private bool inited = false;

    private void Awake()
    {
        // get charator list
        GameManager.OnGameInited(init);

    }

    private void init(GameObject gameManagerObj)
    {
        if (inited == true)
        {
            return;
        }

        inited = true;

        partyManager = gameManagerObj.GetComponent<PartyManager>();

        partyManager.InitedCallback(InitBattleVisual);

    }

    private void InitBattleVisual(PartyManager p)
    {
        partyMembers = partyManager.AllMembers;
        Debug.Log("partyMembers" + partyMembers.Count);
        // ��ʼ����ɫ�Ӿ�Ԫ��
        initCharatorVisual();
        initEnemyVisual();

        battleUI = battleUIDocument.GetComponent<BattleUI>().ShowIU(enemyBattleVisualList);
        battleUI.ShowBattlerLabel(charactorBattleVisualList[currentBattleCharacterIndex].info.Name);

        // ѡ����˻ص�
        battleUI.ActionSelectEnemy += (BattleVisual selectedEnemy) =>
        {
            Debug.Log("enemy selected");
            BattleVisual currentCharacter = charactorBattleVisualList[currentBattleCharacterIndex];
            if (currentCharacter != null)
            {
                currentCharacter.AttackTarget = selectedEnemy;
                currentCharacter.battleAction = BattleVisual.BattleAction.Attack;
                battleQueue.Add(currentCharacter);
                currentBattleCharacterIndex++;

                if (currentBattleCharacterIndex < charactorBattleVisualList.Count) {
                    battleUI.ShowBattlerLabel(charactorBattleVisualList[currentBattleCharacterIndex].info.Name);
                }
            }

            if (currentBattleCharacterIndex >= charactorBattleVisualList.Count)
            {
                battleUI.HideBattlerLabel();
                // ��ʼ�غ��ݳ�
                StartCoroutine(StartBattle());
            }
        };

        // ���ܻص�
        battleUI.ActionRun += () =>
        {
            BattleVisual currentCharacter = charactorBattleVisualList[currentBattleCharacterIndex];
            currentCharacter.battleAction = BattleVisual.BattleAction.Run;
            currentBattleCharacterIndex++;

            if (currentBattleCharacterIndex >= charactorBattleVisualList.Count)
            {
                // ��ʼ�غ��ݳ�
                StartCoroutine(StartBattle());
            }
        };
    }

    private void SetRandomPartyMemberAsAttackTarget()
    {
        for (int i = 0; i < enemyBattleVisualList.Count; i++)
        {
            BattleVisual enemy = enemyBattleVisualList[i];
            int randomIndex = Random.Range(0, charactorBattleVisualList.Count);
            BattleVisual target = charactorBattleVisualList[randomIndex];

            enemy.AttackTarget = target;
            enemy.battleAction = BattleVisual.BattleAction.Attack;

            battleQueue.Add(enemy);
        }
    }

    private IEnumerator StartBattle()
    {
        // �����Զ�ѡ���ҷ���Ϊ���󹥻�
        SetRandomPartyMemberAsAttackTarget();
        // ���а����ٶ�����
        battleQueue.Sort((a, b) =>
        {
            return b.info.Speed - a.info.Speed;
        });
        // ��ʼ�غ�
        Debug.Log("Start Battle");
        for (int i = 0; i < battleQueue.Count; i++)
        {
            BattleVisual current = battleQueue[i];
            BattleVisual.BattleAction action = current.battleAction;
            switch (action)
            {
                case BattleVisual.BattleAction.Attack:
                    yield return StartCoroutine(StartAttackCorotine(current));
                    break;
                case BattleVisual.BattleAction.Run:
                    yield return StartCoroutine(StartRunCorotine(current));
                    break;
                default:
                    break;
            }
        }
        Debug.Log("battle done");
        // �жϻغ��Ƿ���� ʣ����� or ��������
        if (enemyBattleVisualList.Count == 0)
        {
            battleUI.AppendBattleMessage("ս��ʤ����");
            yield return new WaitForSeconds(1);
            // ����״̬
            ResetBattleStatus();
            BackToWorldMap();
            yield break;
        }
        else if (charactorBattleVisualList.Count == 0)
        {
            battleUI.AppendBattleMessage("ս��ʧ�ܣ�");
            yield return new WaitForSeconds(1);
            // ����״̬
            ResetBattleStatus();
            BackToWorldMap();
            yield break;
        }
        // ����״̬
        ResetBattleStatus();
        battleUI.ShowIU(enemyBattleVisualList);
        battleUI.ShowBattlerLabel(charactorBattleVisualList[currentBattleCharacterIndex].info.Name);

        yield break;
    }

    private void ResetBattleStatus()
    {
        // ����״̬
        currentBattleCharacterIndex = 0;
        battleQueue.Clear();
        CurrentBattleStatus = BattleActionStatus.SelectAction;
    }

    private void BackToWorldMap() {
        SceneLoader.LoadAddressableScene(isDebug ? SceneLoader.DEBUG_SCENE : SceneLoader.WORLD_MAP);
    }

    private IEnumerator StartRunCorotine(BattleVisual current)
    {
        int chance = Random.Range(0, 100);
        bool success = chance <= RunChance;
        string text = success ? "�ɹ�" : "ʧ��";
        battleUI.AppendBattleMessage($"����{text}");
        yield return new WaitForSeconds(1);

        // ��ת����
        BackToWorldMap();
        yield break;
    }

    private BattleVisual GetRandomPartyMember()
    {
        if (charactorBattleVisualList.Count > 0)
        {
            return charactorBattleVisualList[Random.Range(0, charactorBattleVisualList.Count)];
        }
        else
        {
            return null;
        }
    }

    private BattleVisual GetRandomEnemy()
    {
        if (enemyBattleVisualList.Count > 0)
        {
            return enemyBattleVisualList[Random.Range(0, enemyBattleVisualList.Count)];
        }
        else
        {
            return null;
        }
    }

    private IEnumerator StartAttackCorotine(BattleVisual current)
    {
        CurrentBattleStatus = BattleManager.BattleActionStatus.InBattle;

        if (current.AttackTarget.info.IsDead)
        {
            BattleVisual newTarget;
            if (current.battlerType == BattleVisual.BattlerType.Enemy)
            {
                newTarget = GetRandomPartyMember();
            }
            else
            {
                newTarget = GetRandomEnemy();
            }
            if (newTarget != null)
            {
                current.AttackTarget = newTarget;
                battleUI.AppendBattleMessage($"{current.AttackTarget.info.Name}����, �������Ŀ�� {newTarget.info.Name}");
            }
            else
            {
                battleUI.AppendBattleMessage($"{current.AttackTarget.info.Name}����, �޹���Ŀ��");
            }
        }

        AttackInfo attackInfo = current.instance.GetComponent<CanAttack>().Attack(current);
        battleUI.AppendBattleMessage($"{attackInfo.CurrentName} ���� {attackInfo.TargetName}, ����˺� {attackInfo.Damage}");

        if (current.AttackTarget.info.CurrentHealth <= 0)
        {
            battleUI.AppendBattleMessage($"{attackInfo.TargetName} ������");
            if (current.AttackTarget.battlerType == BattleVisual.BattlerType.Enemy)
            {
                enemyBattleVisualList.Remove(current.AttackTarget);
            }
            else
            {
                charactorBattleVisualList.Remove(current.AttackTarget);
            }

            Destroy(current.AttackTarget.instance.gameObject);
        }

        yield return new WaitForSeconds(1);
    }

    private void initCharatorVisual()
    {
        GameObject[] partyVisuals = GameObject.FindGameObjectsWithTag(PARTY_MEMBER_KEY);
        //System.Array.Sort(partyVisuals, (a, b) =>
        //{
        //    string name1 = new Regex(@"\d+").Matches(a.name)[0].ToString();
        //    int num1 = int.Parse(name1);

        //    string name2 = new Regex(@"\d+").Matches(a.name)[0].ToString();
        //    int num2 = int.Parse(name2);

        //    return num2 - num1;
        //});

        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (partyVisuals.Length <= i)
            {
                // ��������Ⱦλ��
                break;
            }
            GameObject visualSlot = partyVisuals[i];
            BattleBasicInfos character = partyMembers[i].info;
            GameObject instance = Instantiate(character.BattlePrefab, visualSlot.transform.position, Quaternion.identity);
            instance.GetComponent<SpriteRenderer>().sortingOrder = 1;
            instance.transform.SetParent(visualSlot.transform);
            charactorBattleVisualList.Add(new BattleVisual
            {
                battlerType = BattleVisual.BattlerType.Character,
                info = character,
                instance = instance
            });
        }
    }

    private void initEnemyVisual()
    {
        GameObject[] enemyVisuals = GameObject.FindGameObjectsWithTag(ENEMY_MEMBER_KEY);
        Debug.Log($"enemey slot length: {enemyVisuals.Length}");
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemyVisuals.Length <= i)
            {
                // ��������Ⱦλ��
                break;
            }
            GameObject visualSlot = enemyVisuals[i];
            BattleBasicInfos enemy = enemies[i].info;
            GameObject instance = Instantiate(enemy.BattlePrefab, visualSlot.transform.position, Quaternion.identity);
            instance.GetComponent<SpriteRenderer>().sortingOrder = 1;
            instance.transform.SetParent(visualSlot.transform);
            enemyBattleVisualList.Add(new BattleVisual
            {
                battlerType = BattleVisual.BattlerType.Enemy,
                info = enemy,
                instance = instance
            });
        }
        Debug.Log($"enemy visual list inited, length: {enemyBattleVisualList.Count}");
    }
}

[System.Serializable]
public class BattleVisual
{
    public BattlerType battlerType;
    public BattleBasicInfos info;
    public GameObject instance;

    public BattleVisual AttackTarget;
    public BattleAction battleAction;
    public enum BattleAction
    {
        Attack,
        Skill,
        Run
    }

    public enum BattlerType
    {
        Character,
        Enemy
    }
}

