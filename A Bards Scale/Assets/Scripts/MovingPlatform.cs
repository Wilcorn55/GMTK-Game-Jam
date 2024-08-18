using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    public List<Transform> points;
    public Transform platform;
    int goalPoint = 0;
    public float moveSpeed = 2;

    private bool shouldMove = false; // Flag to control when the platform should move


    // Update is called once per frame
    void Update()
    {

        if (shouldMove)
        {
            MoveToNextPoint();
        }

    }

    public void MoveToNextPoint()
    {
        // Move the platform towards the goal point
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, Time.deltaTime * moveSpeed);

        if (Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
        {
            if (goalPoint == points.Count - 1)
                goalPoint = 0;
            else
                goalPoint++;
        }
    }

    public void StartMovement()
    {
        shouldMove = true;
    }
}
