using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask interactableLayer = -1;

    [Header("Debug")]
    [SerializeField] private InteractableObject currentInteractable;

    private List<InteractableObject> nearbyInteractables = new List<InteractableObject>();
    private GameManager gameManager;
    private ChatSystem chatSystem;
    private PartyManager partyManager;

    private void Awake()
    {

        GameManager.OnGameInited(init);
    }

    void init(GameObject gameManagerObj)
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        chatSystem = gameManager.GetComponent<ChatSystem>();
        partyManager = gameManager.GetComponent<PartyManager>();
    }

    private void Update()
    {
        FindInteractables();
        CheckNearestInteractable();
        DetectPlayerChat();
    }

    private bool isSleep = false;

    private async void HandleChatDone()
    {
        isSleep = true;
        await Task.Delay(200);
        isSleep = false;
    }

    private void HandleJoinParty(CharacterEntity character) {
        partyManager.JoinParty(character);
        Destroy(currentInteractable.gameObject);
        Debug.Log($"{character.info.Name} has joined party");
    }

    private void DetectPlayerChat()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            if (currentInteractable != null)
            {
                Debug.Log("has InteractObj");
                ChatSection chatSection = currentInteractable.GetCurrentChatSection();
                if (chatSection != null && !chatSection.isEmpty())
                {
                    if (chatSystem.IsInConversasion || isSleep) {
                        return;
                    }
                    chatSystem.StartChat(currentInteractable);
                    chatSystem.OnChatDone += HandleChatDone;
                    chatSystem.OnJoinParty += HandleJoinParty;
                }
                else
                {
                    Debug.Log("No Chat Section");
                }

            }
            else
            {

            }
        }
    }

    private void FindInteractables()
    {
        nearbyInteractables.Clear();

        // ���μ�ⷶΧ�ڵĿɽ�������
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            gameObject.transform.position,
            detectionRadius,
            interactableLayer
        );

        foreach (var collider in hitColliders)
        {
            InteractableObject interactable = collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                nearbyInteractables.Add(interactable);
            }
        }
    }

    private void CheckNearestInteractable()
    {
        InteractableObject nearestInteractable = null;
        float nearestDistance = float.MaxValue;

        foreach (var interactable in nearbyInteractables)
        {
            // �������
            float distance = Vector3.Distance(
                gameObject.transform.position,
                GetInteractablePosition(interactable)
            );

            // ����Ƿ��ڽ���������
            if (distance < nearestDistance)
            {
                nearestInteractable = interactable;
                nearestDistance = distance;
            }
        }

        // ���µ�ǰ��������
        if (currentInteractable != nearestInteractable)
        {
            if (currentInteractable != null)
            {
                // ����֮ǰ��ʾ�Ľ���ui
            }

            currentInteractable = nearestInteractable;

            if (currentInteractable != null)
            {
                // ��ʾ������
            }
            else
            {

            }
        }
    }

    private Vector3 GetInteractablePosition(InteractableObject interactable)
    {
        MonoBehaviour monoBehaviour = interactable as MonoBehaviour;
        return monoBehaviour != null ? monoBehaviour.transform.position : Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
