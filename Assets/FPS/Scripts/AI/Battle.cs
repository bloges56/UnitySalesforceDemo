using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Battle : MonoBehaviour
{
    [Header("Battle Waves")]
    [SerializeField] Queue<Queue<KeyValuePair<Enemy, int>>> wavesOfBattle;
    Queue<KeyValuePair<Enemy, int>> currentWave;
    KeyValuePair<Enemy, int> currentSubWave;

    [Header("Spawns")]
    [SerializeField] List<Transform> spawnPoints;

    [Header("Battle Settings")]
    [SerializeField] int waveInterval, enemySpawnInterval;

    bool isWaveSpawning = false;
    bool areEnemiesSpawning = false;

    Dictionary<string, List<Enemy>> _enemiesPool = new Dictionary<string, List<Enemy>>();

    // Start is called before the first frame update
    void Start()
    {
        _enemiesPool.Add("alive", new List<Enemy>());
        _enemiesPool.Add("dead", new List<Enemy>());
       
    }

    Enemy CreateEnemy()
    {
        Enemy createdEnemy =  Instantiate(currentSubWave.Key.gameObject).GetComponent<Enemy>();
        _enemiesPool["alive"].Add(createdEnemy);
        return createdEnemy;
    }

   Enemy GetEnemy()
    {
        foreach(Enemy enm in _enemiesPool["dead"])
        {
            if(enm.EnemyType.Equals(currentSubWave.Key.EnemyType))
            {
                _enemiesPool["alive"].Add(enm);
                _enemiesPool["dead"].Remove(enm);
                enm.gameObject.SetActive(true);
                return enm;
            }
        }
        return CreateEnemy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(RunBattle());
        }
    }

    IEnumerator RunBattle()
    {
        while(wavesOfBattle.Any())
        {  
            currentWave = wavesOfBattle.Dequeue();
            isWaveSpawning = true;
            StartCoroutine(SpawnWave());
            yield return new WaitUntil(() => _enemiesPool["alive"].Count == 0 && !isWaveSpawning);
            yield return new WaitForSeconds(waveInterval);
        }
    }

    IEnumerator SpawnWave()
    {

        currentSubWave = currentWave.Dequeue();
        areEnemiesSpawning = true;
        StartCoroutine(SpawnEnemies());

        yield return new WaitUntil(() => !areEnemiesSpawning && _enemiesPool["alive"].Count == 0);

        isWaveSpawning= false;
    }


    IEnumerator SpawnEnemies()
    {
        for(int i =0; i < currentSubWave.Value; i++)
        {
            Enemy spawnedEnemy = GetEnemy();
            SpawnEnemy(spawnedEnemy);
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    void SpawnEnemy(Enemy enm)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count - 1);
        enm.gameObject.transform.position = spawnPoints[spawnIndex].position;
    }

    public void RemoveEnemy(Enemy enm)
    {
        enm.gameObject.SetActive(false);
        _enemiesPool["alive"].Remove(enm);
        _enemiesPool["dead"].Remove(enm);
    }
}
