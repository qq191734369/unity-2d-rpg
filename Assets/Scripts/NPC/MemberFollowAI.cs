using UnityEngine;

public class MemberFollowAI : MonoBehaviour
{
    [SerializeField] private int speed = 10;

    [Header("Debug")]
    [SerializeField] private Transform followTarget;

    private float followDist;
    private Animator followAnimator;
    private SpriteRenderer followRenderer;

    private const string IS_WALKING_KEY = "isWalking";
    private const string BATTLE_SCENE = "BattleScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        followRenderer = GetComponent<SpriteRenderer>();

        followTarget = GameObject.FindFirstObjectByType<PlayerMove>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, followTarget.position) > followDist)
        {
            // walk to the player
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, followTarget.position, step);

            if (followTarget.position.x - transform.position.x < 0)
            {
                followRenderer.flipX = true;
            }
            else
            {
                followRenderer.flipX = false;
            }
        }
        else
        {
            // stop walking, animator to idle
            
        }
    }

    public void SetFollowDistance(float distance)
    {
        followDist = distance;
    }
}
