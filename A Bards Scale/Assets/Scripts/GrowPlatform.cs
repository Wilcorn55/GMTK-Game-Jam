using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlatform : MonoBehaviour
{
    public float growthIncrement = 0.5f; // Total growth increment in each direction
    public float growthDuration = 2f;    // Duration over which the growth happens
    public float maxScale = 2.5f;        // Maximum allowed scale to prevent excessive growth
    public float shrinkDelay = 5f;       // Time before the platform shrinks back
    public float closeEnoughThreshold = 0.001f; // Threshold to snap to the final scale

    private Vector3 initialScale;        // To store the original scale of the platform
    private Coroutine growthCoroutine;

    private bool isGrowingOrShrinking = false;  // Flag to track if growth/shrink is in progress

    public bool IsGrowingOrShrinking
    {
        get { return isGrowingOrShrinking; }
    }

    void Start()
    {
        // Store the initial scale at the start
        initialScale = transform.localScale;
    }

    // Method to start the gradual growth of the platform
    public void Grow()
    {
        // If a growth or shrink process is already ongoing, don't start another
        if (isGrowingOrShrinking)
            return;

        // Set the flag to indicate that a growth/shrink process is in progress
        isGrowingOrShrinking = true;

        // Start the growth coroutine
        growthCoroutine = StartCoroutine(GrowAndShrinkOverTime());
    }

    private IEnumerator GrowAndShrinkOverTime()
    {
        Vector3 targetScale = initialScale + new Vector3(growthIncrement, growthIncrement, growthIncrement);
        targetScale.x = Mathf.Min(targetScale.x, maxScale);
        targetScale.y = Mathf.Min(targetScale.y, maxScale);
        targetScale.z = Mathf.Min(targetScale.z, maxScale);

        float elapsedTime = 0f;

        while (elapsedTime < growthDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Mathf.Clamp01(elapsedTime / growthDuration));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localScale = targetScale;
        yield return new WaitForSeconds(shrinkDelay);

        elapsedTime = 0f;

        while (elapsedTime < growthDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale, Mathf.Clamp01(elapsedTime / growthDuration));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localScale = initialScale;
        isGrowingOrShrinking = false;
    }
}

