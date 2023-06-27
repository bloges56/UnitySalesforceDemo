using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string EnemyType;
    [HideInInspector] public Battle battle;

    private void OnDestroy()
    {
        try
        {
            battle.enemiesAlive--;
        }
        catch(Exception e)
        {
            Debug.Log("Battle is over");
        }
        
    }
}
