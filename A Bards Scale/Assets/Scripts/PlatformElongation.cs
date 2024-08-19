using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformElongation : MonoBehaviour
{
    public GameObject platformPrefab;  // Prefab of the platform to be cloned
    public int maxSegments = 5;        // Maximum number of segments to add on each side
    public float segmentWidth = 1f;    // Width of each platform segment
    public float moveDuration = 0.5f;  // Time it takes for the platform to move to its final position

    private int currentSegments = 0;   // Tracks how many segments have been added on each side

    public void ElongatePlatform()
    {
        if (currentSegments < maxSegments)
        {
            // Spawn a new segment under the current platform for the right side
            Vector3 initialPositionRight = transform.position;
            Vector3 targetPositionRight = new Vector3(transform.position.x + segmentWidth * (currentSegments + 1), transform.position.y, transform.position.z);
            StartCoroutine(MovePlatformSegment(initialPositionRight, targetPositionRight));

            // Spawn a new segment under the current platform for the left side
            Vector3 initialPositionLeft = transform.position;
            Vector3 targetPositionLeft = new Vector3(transform.position.x - segmentWidth * (currentSegments + 1), transform.position.y, transform.position.z);
            StartCoroutine(MovePlatformSegment(initialPositionLeft, targetPositionLeft));

            currentSegments++;
        }
    }

    private IEnumerator MovePlatformSegment(Vector3 initialPosition, Vector3 targetPosition)
    {
        GameObject newSegment = Instantiate(platformPrefab, initialPosition, transform.rotation);
        newSegment.transform.SetParent(transform.parent); // Set the same parent as the original platform

        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            newSegment.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        newSegment.transform.position = targetPosition;
    }
}
