using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformElongation : MonoBehaviour
{
    public GameObject platformPrefab;  // Prefab of the platform to be cloned
    public int maxSegments = 5;        // Maximum number of segments to add on each side
    public float segmentWidth = 1f;    // Width of each platform segment

    private int currentSegments = 0;   // Tracks how many segments have been added on each side

    public void ElongatePlatform()
    {
        if (currentSegments < maxSegments)
        {
            // Create a new segment on the right side
            Vector3 newPositionRight = new Vector3(transform.position.x + segmentWidth * (currentSegments + 1), transform.position.y, transform.position.z);
            GameObject rightSegment = Instantiate(platformPrefab, newPositionRight, transform.rotation);
            rightSegment.transform.SetParent(transform.parent); // Ensure it is under the same parent

            // Create a new segment on the left side
            Vector3 newPositionLeft = new Vector3(transform.position.x - segmentWidth * (currentSegments + 1), transform.position.y, transform.position.z);
            GameObject leftSegment = Instantiate(platformPrefab, newPositionLeft, transform.rotation);
            leftSegment.transform.SetParent(transform.parent); // Ensure it is under the same parent

            currentSegments++;
        }
    }
}
