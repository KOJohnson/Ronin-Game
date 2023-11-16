using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatManager : MonoBehaviour
{
    public static EnemyCombatManager Instance;
    
    [SerializeField]private List<EnemyController> enemiesInCombat;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddToList(EnemyController enemyToAdd)
    {
        enemiesInCombat.Add(enemyToAdd);
    }
    
    public void RemoveFromList(EnemyController enemyToAdd)
    {
        enemiesInCombat.Remove(enemyToAdd);
    }
    
}
