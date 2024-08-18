using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public List<Transform> points;
    public Transform platform;
    int goalPoint = 0;
    public float moveSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint() 
    {
        //move the platform towards the goal point
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, 1*Time.deltaTime*moveSpeed);

        if(Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
        {

        }
    }
}
