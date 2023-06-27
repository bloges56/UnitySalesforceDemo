using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [HideInInspector] public BossBattle battle;

    private void OnDestroy()
    {
        try
        {
            battle.isBossDead = true;
        }
        catch 
        {
            Debug.Log("No Battle");
        }
    }

}
