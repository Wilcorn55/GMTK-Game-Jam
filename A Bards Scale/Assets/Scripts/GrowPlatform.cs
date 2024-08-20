using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlatform : MonoBehaviour
{
    public float growthIncrement = 0.5f; 
    public float growthDuration = 2f;    
    public float maxScale = 2.5f;        
    public float shrinkDelay = 5f;    
    public float closeEnoughThreshold = 0.001f; 

    private Vector3 initialScale;       
    private Coroutine growthCoroutine;

    private bool isGrowingOrShrinking = false;  

    public bool IsGrowingOrShrinking
    {
        get { return isGrowingOrShrinking; }
    }

    void Start()
    {
        //Store the scale
        initialScale = transform.localScale;
    }

    
    public void Grow()
    {
        
        if (isGrowingOrShrinking)
            return;

        
        isGrowingOrShrinking = true;

        
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

