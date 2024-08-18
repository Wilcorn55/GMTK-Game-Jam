using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{

    public Transform detectionPoint;

    private const float basicDetectionRadius = 0.2f;

    public LayerMask detectionLayer;



    // Update is called once per frame
    void Update()
    {
        if (ObjectTrigger())
        {
            if (InteractInput()) 
            {
                Debug.Log("Interacting");
            }
        
        
        }
    }

    bool InteractInput() 
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool ObjectTrigger() 
    {
        return Physics2D.OverlapCircle(detectionPoint.position, basicDetectionRadius, detectionLayer);     
    }
}
