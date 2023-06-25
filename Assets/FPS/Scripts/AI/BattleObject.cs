using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BattleObject", order = 1)]
public class BattleObject : ScriptableObject
{
   public List<WaveObject> battle;
}
