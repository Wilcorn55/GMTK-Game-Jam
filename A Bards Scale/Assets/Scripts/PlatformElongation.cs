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

    private bool isElongating = false; // Flag to indicate if elongation is in progress

    public bool IsElongating
    {
        get { return isElongating; }
    }
    public void ElongatePlatform()
    {
        if (currentSegments < maxSegments && !isElongating)
        {
            isElongating = true; // Set the flag when elongation starts

            // Start the elongation process for both sides
            StartCoroutine(ElongateBothSides());
        }
    }

    private IEnumerator MovePlatformSegment(Vector3 initialPosition, Vector3 targetPosition)
    {
        GameObject newSegment = Instantiate(platformPrefab, initialPosition, transform.rotation);
        newSegment.transform.SetParent(transform.parent);

        // Adjust the scale to match the original exactly
        Vector3 originalScale = transform.Find("Platform").localScale;
        newSegment.transform.localScale = originalScale;

        // Adjust the order in layer to be behind the original
        SpriteRenderer newSegmentRenderer = newSegment.GetComponent<SpriteRenderer>();
        SpriteRenderer originalRenderer = transform.Find("Platform").GetComponent<SpriteRenderer>();

        if (newSegmentRenderer != null && originalRenderer != null)
        {
            newSegmentRenderer.sortingOrder = originalRenderer.sortingOrder - 1;
        }

        // Adjust position based on potential pivot differences
        float yOffset = originalRenderer.bounds.extents.y - newSegmentRenderer.bounds.extents.y;
        initialPosition.y -= yOffset;
        targetPosition.y -= yOffset;

        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            newSegment.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        newSegment.transform.position = targetPosition;
    }
    private IEnumerator ElongateBothSides()
    {
        // Spawn and move the right segment
        Vector3 initialPositionRight = transform.position;
        Vector3 targetPositionRight = new Vector3(transform.position.x + segmentWidth * (currentSegments + 1), transform.position.y, transform.position.z);
        yield return StartCoroutine(MovePlatformSegment(initialPositionRight, targetPositionRight));

        // Spawn and move the left segment
        Vector3 initialPositionLeft = transform.position;
        Vector3 targetPositionLeft = new Vector3(transform.position.x - segmentWidth * (currentSegments + 1), transform.position.y, transform.position.z);
        yield return StartCoroutine(MovePlatformSegment(initialPositionLeft, targetPositionLeft));

        // Increment the segment count after elongation is complete
        currentSegments++;

        // Clear the flag when elongation is done
        isElongating = false;
    }
}
