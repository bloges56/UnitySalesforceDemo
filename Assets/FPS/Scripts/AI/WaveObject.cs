using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WaveObject", order = 1)]
public class WaveObject : ScriptableObject
{
    public List<SubWaveObject> wave;
}
