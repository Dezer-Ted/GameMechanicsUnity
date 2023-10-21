using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> possibleFood = new List<GameObject>();
    [SerializeField]
    float spawnRange;
    [SerializeField]
    float minSpawnCooldown, maxSpawnCooldown;
    void Start()
    {
        SpawnFood();
        StartCoroutine(WaitForSpawnCycle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnFood() 
    {
        int spread = Random.Range(3, 6) * 5;
        for (int index = 0; index < 360; ++index)
        {
            Vector3 pos;
            if(index % spread == 0)
            {
                pos = new Vector3
                (
                    Mathf.Cos(index) * spawnRange,
                    Mathf.Sin(index) * spawnRange,
                    -0.5f
                );
                Instantiate(possibleFood[Random.Range(0,possibleFood.Count-1)], pos, Quaternion.identity);
            }
        }
    }

    IEnumerator WaitForSpawnCycle()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnCooldown, maxSpawnCooldown));
        SpawnFood();
        StartCoroutine(WaitForSpawnCycle());

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
        }
    }
}
