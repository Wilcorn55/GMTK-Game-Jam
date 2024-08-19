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

    private float inputTimeout = 2f;
    private float inputTimer;

    // Reference to the MovingPlatform script
    private MovingPlatform currentPlatform;
    

    void Update()
    {
        DetectPlatform();

        if (currentPlatform != null && ObjectTrigger())
        {
            RegisterInput();
            CheckCombination();
        }
    }

    bool ObjectTrigger()
    {
        Collider2D collider = Physics2D.OverlapCircle(detectionPoint.position, basicDetectionRadius, detectionLayer);
        if (collider != null)
        {

            return true;
        }
        else
        {

            return false;
        }
        
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
        if (CheckSequence(elongateCombination))  // Add a new combination for elongation
        {
            Debug.Log("Elongate Combination Triggered");
            if (currentPlatform != null)
            {
                PlatformElongation elongationScript = currentPlatform.GetComponent<PlatformElongation>();
                if (elongationScript != null)
                {
                    elongationScript.ElongatePlatform();
                }
            }

        }
        else if (CheckSequence(moveCombination))
        {
            Debug.Log("Move Combination Triggered");
            // Trigger platform movement
            if (currentPlatform != null)
            {
                currentPlatform.StartMovement();  // Start movement for the current platform
            }
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
        // Perform a raycast to detect the platform
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
}

