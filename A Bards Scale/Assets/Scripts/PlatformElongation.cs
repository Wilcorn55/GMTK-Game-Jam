using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformElongation : MonoBehaviour
{
    public GameObject platformPrefab;  
    public int maxSegments = 5;        
    public float segmentWidth = 1f;   
    public float moveDuration = 0.5f;  

    private int currentSegments = 0;  

    private bool isElongating = false; 

    public bool IsElongating
    {
        get { return isElongating; }
    }
    public void ElongatePlatform()
    {
        if (currentSegments < maxSegments && !isElongating)
        {
            isElongating = true; //Set when elongation starts

            //Start the elongation
            StartCoroutine(ElongateBothSides());
        }
    }

    private IEnumerator MovePlatformSegment(Vector3 initialPosition, Vector3 targetPosition)
    {
        GameObject newSegment = Instantiate(platformPrefab, initialPosition, transform.rotation);
        newSegment.transform.SetParent(transform.parent);

        //Adjust the scale
        Vector3 originalScale = transform.Find("Platform").localScale;
        newSegment.transform.localScale = originalScale;

        //adjust the order in layer
        SpriteRenderer newSegmentRenderer = newSegment.GetComponent<SpriteRenderer>();
        SpriteRenderer originalRenderer = transform.Find("Platform").GetComponent<SpriteRenderer>();

        if (newSegmentRenderer != null && originalRenderer != null)
        {
            newSegmentRenderer.sortingOrder = originalRenderer.sortingOrder - 1;
        }

      
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
        
        Vector3 initialPositionRight = transform.position;
        Vector3 targetPositionRight = new Vector3(transform.position.x + segmentWidth * (currentSegments + 1), transform.position.y, transform.position.z);
        yield return StartCoroutine(MovePlatformSegment(initialPositionRight, targetPositionRight));

        
        Vector3 initialPositionLeft = transform.position;
        Vector3 targetPositionLeft = new Vector3(transform.position.x - segmentWidth * (currentSegments + 1), transform.position.y, transform.position.z);
        yield return StartCoroutine(MovePlatformSegment(initialPositionLeft, targetPositionLeft));

       
        currentSegments++;

        // Clear
        isElongating = false;
    }
}
