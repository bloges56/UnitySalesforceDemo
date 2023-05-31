using Salesforce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int spawnInterval;
    float timeLeft;
    [SerializeField] SalesforceClient salesforceClient;

    int count = 0;

    Dictionary<GameObject, Enemy> enemies = new Dictionary<GameObject, Enemy>();

    // Update is called once per frame
    void Update()
    {
        if(timeLeft <= 0)
        {
            Enemy newEnemy = new Enemy();
            newEnemy.name = "Enemy Clone " + count;
            StartCoroutine(CreateEnemy(newEnemy));
            GameObject newEnemyObject = Instantiate(enemyPrefab, this.transform);
            enemies.Add(newEnemyObject, newEnemy);
            timeLeft = spawnInterval;
            count++;
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }

    IEnumerator CreateEnemy(Enemy enemy)
    {
        HandleLogin();
        Coroutine<Enemy> insertRecordRoutine = this.StartCoroutine<Enemy>(
            salesforceClient.insert(enemy)
        );
        yield return insertRecordRoutine.coroutine;
    }

    IEnumerator HandleLogin()
    {
        Coroutine<bool> loginRoutine = this.StartCoroutine<bool>(
        salesforceClient.login()
    );
        yield return loginRoutine.coroutine;
        try
        {
            loginRoutine.getValue();
            Debug.Log("Salesforce login successful.");
        }
        catch (SalesforceConfigurationException e)
        {
            Debug.Log("Salesforce login failed due to invalid auth configuration");
            throw e;
        }
        catch (SalesforceAuthenticationException e)
        {
            Debug.Log("Salesforce login failed due to invalid credentials");
            throw e;
        }
        catch (SalesforceApiException e)
        {
            Debug.Log("Salesforce login failed");
            throw e;
        }
    }

    IEnumerator DeleteRecord(Enemy enemy)
    {
        HandleLogin();
        Coroutine<Enemy> deleteRecordRoutine = this.StartCoroutine<Enemy>(
            salesforceClient.delete(enemy)
        );
        yield return deleteRecordRoutine.coroutine;

    }

    public void RemoveEnemy(GameObject enemyObject)
    {
        Enemy enemyToRemove = enemies[enemyObject];
        enemies.Remove(enemyObject);
        Destroy(enemyObject);
        StartCoroutine(DeleteRecord(enemyToRemove));
    }
}
