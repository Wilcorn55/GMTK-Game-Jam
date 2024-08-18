using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public Transform detectionPoint;
    private const float basicDetectionRadius = 0.2f;
    public LayerMask detectionLayer;

    private Queue<KeyCode> inputSequence = new Queue<KeyCode>();
    private KeyCode[] growCombination = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.UpArrow };
    private KeyCode[] moveCombination = { KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.LeftArrow };

    private float inputTimeout = 2f;
    private float inputTimer;

    // Reference to the MovingPlatform script
    public MovingPlatform movingPlatform;

    void Update()
    {
        if (ObjectTrigger())
        {
            RegisterInput();
            CheckCombination();
        }
    }

    bool ObjectTrigger()
    {
        return Physics2D.OverlapCircle(detectionPoint.position, basicDetectionRadius, detectionLayer);
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
        if (inputSequence.Count > growCombination.Length)
        {
            inputSequence.Dequeue();
        }
    }

    void CheckCombination()
    {
        if (CheckSequence(growCombination))
        {
            Debug.Log("Grow Combination Triggered");
            // Call the method to grow the platform here
        }
        else if (CheckSequence(moveCombination))
        {
            Debug.Log("Move Combination Triggered");
            // Trigger platform movement
            movingPlatform.MoveToNextPoint();
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
}
