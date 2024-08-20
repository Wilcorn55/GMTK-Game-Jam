using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    public Transform[] points;
    public Transform platform;
    int goalPoint = 0;
    public float moveSpeed = 2f;

    private bool shouldMove = false; //control when the platform should move

    public bool isMoving
    {
        get { return shouldMove; }
    }



    //Update is called once per frame
    void Update()
    {

        if (shouldMove)
        {
            MoveToNextPoint();
        }

    }

    public void MoveToNextPoint()
    {
        if (points.Length == 0) return;

        transform.position = Vector3.MoveTowards(transform.position, points[goalPoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, points[goalPoint].position) < 0.1f)
        {
            goalPoint = (goalPoint + 1) % points.Length;
        }
    }

    public void StartMovement()
    {
        shouldMove = true;
    }
    public void StopMovement()
    {
        shouldMove = false;
    }
    
}
