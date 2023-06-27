using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.FPS.AI;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Enemies")]
    [SerializeField] GameObject boss;
    [SerializeField] GameObject normalEnemy;

    [Header("Battle Config")]
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] GameObject doors;
    [SerializeField] int enemySpawnInterval;

    [HideInInspector] public bool isBossDead = false;

    private List<GameObject> enemies;

    private void Start()
    {
        boss.GetComponent<Boss>().battle = this;
        enemies = new List<GameObject>();
    }

    void CloseDoors()
    {
        doors.SetActive(true);
    }

    void OpenDoors()
    {
        Destroy(doors);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CloseDoors();
            StartCoroutine(SpawnEnemies());
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (!isBossDead)
        {
            GameObject spawnedEnemy = Instantiate(normalEnemy);
            enemies.Add(spawnedEnemy);
            SpawnEnemy(spawnedEnemy);
            yield return new WaitForSeconds(enemySpawnInterval);
        }
        KillRestOfEnemies();
        OpenDoors();
        yield return new WaitUntil(() => !enemies.Any());
        Destroy(spawnPoints[0].parent.gameObject);
        Destroy(gameObject);
    }
    void SpawnEnemy(GameObject enm)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count - 1);
        enm.transform.position = spawnPoints[spawnIndex].position;
    }

    void KillRestOfEnemies()
    {
        while(enemies.Any())
        {
            enemies[0].GetComponent<EnemyController>().OnDie();
            enemies.RemoveAt(0);
        }
    }
}
