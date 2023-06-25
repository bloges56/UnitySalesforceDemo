using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Battle : MonoBehaviour
{
    [Header("Battle Waves")]
    [SerializeField] BattleObject wavesOfBattle;
    WaveObject currentWave;
    SubWaveObject currentSubWave;

    [Header("Spawns")]
    [SerializeField] List<Transform> spawnPoints;

    [Header("Battle Settings")]
    [SerializeField] int waveInterval, enemySpawnInterval;

    bool isWaveSpawning = false;
    bool areEnemiesSpawning = false;

    [HideInInspector]
    public int enemiesAlive = 0;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(RunBattle());
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator RunBattle()
    {
        foreach(WaveObject wave in wavesOfBattle.battle)
        {  
            currentWave = wave;
            isWaveSpawning = true;
            StartCoroutine(SpawnWave());
            yield return new WaitUntil(() => !isWaveSpawning  && enemiesAlive == 0);
            yield return new WaitForSeconds(waveInterval);
        }
        Destroy(spawnPoints[0].parent.gameObject);
        Destroy(gameObject);
    }

    IEnumerator SpawnWave()
    {
        foreach (SubWaveObject subWave in currentWave.wave)
        {
            currentSubWave = subWave;
            areEnemiesSpawning = true;
            StartCoroutine(SpawnEnemies());
            yield return new WaitUntil(() => !areEnemiesSpawning);
        }  
        isWaveSpawning= false;
    }


    IEnumerator SpawnEnemies()
    {
        for(int i =0; i < currentSubWave.numberofEnemies; i++)
        {
            Enemy spawnedEnemy = Instantiate(currentSubWave.enemy.gameObject).GetComponent<Enemy>();
            SpawnEnemy(spawnedEnemy);
            spawnedEnemy.battle = this;
            enemiesAlive++;
            yield return new WaitForSeconds(enemySpawnInterval);
        }
        areEnemiesSpawning = false;
    }

    void SpawnEnemy(Enemy enm)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count - 1);
        enm.gameObject.transform.position = spawnPoints[spawnIndex].position;
    }
}
