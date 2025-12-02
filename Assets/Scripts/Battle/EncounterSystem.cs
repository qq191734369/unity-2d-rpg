using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EncounterSystem : MonoBehaviour
{
    private const float TIME_PER_STEP = 0.5f;
    [SerializeField]
    public LayerMask enemyLayer = -1;

    [SerializeField]
    public int maxStepsToEncounter;

    [SerializeField]
    public int minStepsToEncounter;

    bool movingInGrass = false;
    private Vector2 movement;
    private float stepTimer;
    private int stepsInGrass = 0;
    private int stepsToEncounter = 0;
    private PlayerMove playerMove;

    private void Awake()
    {
        CalculateStepsToNextEncounter();
        playerMove = GetComponent<PlayerMove>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1, enemyLayer);
        movingInGrass = colliders.Length != 0 && playerMove.GetPlayerMove() != Vector2.zero;

        if (movingInGrass == true)
        {
            stepTimer += Time.fixedDeltaTime;
            if (stepTimer > TIME_PER_STEP)
            {
                stepsInGrass++;
                stepTimer = 0;

                // check to see if reach an encounter
                // change sence
                if (stepsInGrass >= stepsToEncounter)
                {
                    Debug.Log($"Start battle, stepsInGrass: {stepsInGrass}");
                    stepsInGrass = 0;
                    CalculateStepsToNextEncounter();
                    EnemyLayer layerInstance = colliders[0].GetComponent<EnemyLayer>();
                    layerInstance?.StartBattle();
                }
            }
        }
    }

    private void CalculateStepsToNextEncounter()
    {
        stepsToEncounter = Random.Range(minStepsToEncounter, maxStepsToEncounter);
    }
}
