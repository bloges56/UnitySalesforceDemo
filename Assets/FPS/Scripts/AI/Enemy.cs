using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string EnemyType;
    public Battle battle;

    private void OnDestroy()
    {
        battle.enemiesAlive--;
    }
}
