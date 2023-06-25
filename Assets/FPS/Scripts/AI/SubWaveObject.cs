using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SubWaveObject", order = 1)]
public class SubWaveObject : ScriptableObject
{
    public Enemy enemy;

    public int numberofEnemies;
}
