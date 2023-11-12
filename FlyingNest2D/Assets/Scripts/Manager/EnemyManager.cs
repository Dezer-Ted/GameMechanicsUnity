using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    float spawnRange = 10;
    [SerializeField]
    int enemiesPerWave = 3;
    [SerializeField]
    float spawnCooldown = 15;
    [SerializeField]
    GameObject enemy;
    void Start()
    {
        StartCoroutine(SpawnRythm());
    }
    //Spawns enemies evenly spread on a circle around (0,0)
    void SpawnEnemies()
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<BirdFlyingBehavior>().CurrentState == BirdFlyingBehavior.PlayerState.inNest)
        {
            StartCoroutine(SpawnRythm());
            return;
        }
        int angle = 0;
        int angleIncrease = 360 / enemiesPerWave;
        Vector2 enemyPos;
        for (int i = 0; i < enemiesPerWave; i++)
        {
            enemyPos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRange, Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRange);
            Instantiate(enemy, enemyPos, Quaternion.identity);
            angle += angleIncrease;
        }
        StartCoroutine(SpawnRythm());
    }
    IEnumerator SpawnRythm ()
    {
        yield return new WaitForSeconds(spawnCooldown);
        SpawnEnemies();
    }
}
