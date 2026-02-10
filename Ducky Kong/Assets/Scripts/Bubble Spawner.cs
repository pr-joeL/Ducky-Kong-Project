using UnityEngine;
using System.Collections;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float bubbleDelayTimeMinimum;
    [SerializeField] private float bubbleDelayTimeMaximum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBubble();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnBubble()
    {
        if (bubblePrefab != null)
        {
            Instantiate(bubblePrefab, transform.position, Quaternion.identity);
            StartCoroutine(SpawnDelay());
        }
        
    }

    private float GetRandomTime()
    {
        return Random.Range(bubbleDelayTimeMinimum, bubbleDelayTimeMaximum);
    }
    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(GetRandomTime());

        SpawnBubble();
    }
}
