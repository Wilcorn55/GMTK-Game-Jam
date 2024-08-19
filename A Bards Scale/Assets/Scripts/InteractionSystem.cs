using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public Transform detectionPoint;
    private const float basicDetectionRadius = 0.2f;
    public LayerMask detectionLayer;
    public List<MovingPlatform> platforms;

    private Queue<KeyCode> inputSequence = new Queue<KeyCode>();
    private KeyCode[] elongateCombination = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.UpArrow };
    private KeyCode[] moveCombination = { KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.LeftArrow };
    private KeyCode[] growCombination = { KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.UpArrow };
    private KeyCode[] destroyCombination = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

    private float inputTimeout = 2f;
    private float inputTimer;

    // Reference to the MovingPlatform script
    private MovingPlatform currentPlatform;
    public GrowPlatform growPlatform;

    private bool isInputProcessed = false;
    private enum InteractionType { None, Grow, Move, Elongate, Destroy }
    private InteractionType currentInteraction = InteractionType.None;

    void Update()
    {
        DetectPlatform();

        if (currentPlatform != null && ObjectTrigger() && !isInputProcessed)
        {
            RegisterInput();
            if (inputSequence.Count > 0)
            {
                StartCoroutine(DelayedCheckCombination());
            }
        }

        // Reset the input flag after the interaction is complete
        if (isInputProcessed && currentInteraction != InteractionType.None && !IsInteractionInProgress())
        {
            ResetInputProcessing();
        }
    }

    bool ObjectTrigger()
    {
        Collider2D collider = Physics2D.OverlapCircle(detectionPoint.position, basicDetectionRadius, detectionLayer);
        return collider != null;

    }

    void RegisterInput()
    {
        // Check for arrow key presses
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputSequence.Enqueue(KeyCode.LeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputSequence.Enqueue(KeyCode.RightArrow);

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputSequence.Enqueue(KeyCode.UpArrow);

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputSequence.Enqueue(KeyCode.DownArrow);

        }

        // Reset the timer whenever a new key is pressed
        if (inputSequence.Count > 0)
        {
            inputTimer = inputTimeout;
        }

        // Limit the size of the queue to the longest combination
        if (inputSequence.Count > elongateCombination.Length)
        {
            inputSequence.Dequeue();
        }
    }

    void CheckCombination()
    {
        if (CheckSequence(elongateCombination))  // Elongate combination
        {
            Debug.Log("Elongate Combination Triggered");
            StartInteraction(InteractionType.Elongate);
        }
        else if (CheckSequence(moveCombination))  // Move combination
        {
            Debug.Log("Move Combination Triggered");
            StartInteraction(InteractionType.Move);
        }
        else if (CheckSequence(growCombination))  // Grow combination
        {
            Debug.Log("Grow Combination Triggered");
            StartInteraction(InteractionType.Grow);
        }
        else if (CheckSequence(destroyCombination))  // Destroy combination
        {
            Debug.Log("Destroy Combination Triggered");
            StartInteraction(InteractionType.Destroy);
        }
    }

    bool CheckSequence(KeyCode[] combination)
    {
        if (inputSequence.Count != combination.Length)
            return false;

        KeyCode[] inputArray = inputSequence.ToArray();
        for (int i = 0; i < combination.Length; i++)
        {
            if (inputArray[i] != combination[i])
                return false;
        }
        return true;
    }
    void DetectPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, detectionLayer);

        if (hit.collider != null)
        {
            MovingPlatform detectedPlatform = hit.collider.GetComponentInParent<MovingPlatform>();

            if (detectedPlatform != null && detectedPlatform != currentPlatform)
            {
                // Stop the movement of the previous platform
                if (currentPlatform != null)
                {
                    currentPlatform.StopMovement();
                }

                // Set the new current platform and clear the input sequence
                currentPlatform = detectedPlatform;
                inputSequence.Clear(); // Clear the input sequence
            }
        }
        else
        {
            // Stop the movement if no platform is detected
            if (currentPlatform != null)
            {
                currentPlatform.StopMovement();
                currentPlatform = null;
                inputSequence.Clear(); // Clear the input sequence
            }
        }
    }
    void StartInteraction(InteractionType interaction)
    {
        if (isInputProcessed)
            return;

        isInputProcessed = true;
        currentInteraction = interaction;

        switch (interaction)
        {
            case InteractionType.Elongate:
                PlatformElongation elongationScript = currentPlatform.GetComponent<PlatformElongation>();
                if (elongationScript != null)
                {
                    elongationScript.ElongatePlatform();
                }
                break;

            case InteractionType.Move:
                currentPlatform.StartMovement();
                break;

            case InteractionType.Grow:
                GrowPlatform growScript = currentPlatform.GetComponent<GrowPlatform>();
                if (growScript != null)
                {
                    growScript.Grow();
                }
                break;

            case InteractionType.Destroy:
                // Implement your destroy logic here
                Debug.Log("Destroy interaction logic not implemented yet.");
                break;
        }
        inputSequence.Clear();
    }
    private IEnumerator DelayedCheckCombination()
    {
        yield return new WaitForSeconds(0.1f); // Small delay before checking
        CheckCombination();
    }
    private void ResetInputProcessing()
    {
        isInputProcessed = false;
        currentInteraction = InteractionType.None;
        inputSequence.Clear(); // Clear input sequence for the next input
    }

    private bool IsInteractionInProgress()
    {
        switch (currentInteraction)
        {
            case InteractionType.Elongate:
                return currentPlatform.GetComponent<PlatformElongation>()?.IsElongating ?? false;
            case InteractionType.Move:
                return currentPlatform.isMoving;  
            case InteractionType.Grow:
                return currentPlatform.GetComponent<GrowPlatform>()?.IsGrowingOrShrinking ?? false;
            case InteractionType.Destroy:
                // Implement your destroy check logic here
                return false;
            default:
                return false;
        }
    }
}

