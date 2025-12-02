using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Vector2 playerMove;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    private ChatUIScript chatUIScript;

    private UIManager uiManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameManager.OnGameInited(init);
    }

    void init(GameObject gameManagerObj)
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        uiManager = gameManagerObj.GetComponent<UIManager>();
        chatUIScript = uiManager.ChatUIDocument.GetComponent<ChatUIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetPlayerMove()
    {
        return playerMove;
    }


    void OnMove(InputValue inputValue)
    {
        if (GameManager.IsPaused)
        {
            return;
        }

        if (chatUIScript.isActive)
        {
            playerMove = default;
            return;
        }
        playerMove = inputValue.Get<Vector2>();

        // 控制显示方向
        if (playerMove.x > 0)
        {
            spriteRenderer.flipX = false;
        } else if (playerMove.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (playerMove != Vector2.zero)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            rb.MovePosition(position + playerMove * speed * Time.fixedDeltaTime);
        }
    }
}