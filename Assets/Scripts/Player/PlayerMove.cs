using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Vector2 playerMove;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private List<InteractableObject> nearbyInteractables = new List<InteractableObject>();
    private GameManager gameManager;
    private ChatUIScript chatUIScript;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameManager.OnGameInited(init);
    }

    void init(GameObject gameManagerObj)
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        chatUIScript = gameManager.ChatUIDocument.GetComponent<ChatUIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(InputValue inputValue)
    {
        if (chatUIScript.isActive)
        {
            playerMove = default;
            return;
        }
        playerMove = inputValue.Get<Vector2>();

        // 控制显示方向
        if (playerMove.x > 0)
        {
            spriteRenderer.flipX = true;
        } else if (playerMove.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        if (playerMove != Vector2.zero)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            rigidbody.MovePosition(position + playerMove * speed * Time.fixedDeltaTime);
        }
    }
}