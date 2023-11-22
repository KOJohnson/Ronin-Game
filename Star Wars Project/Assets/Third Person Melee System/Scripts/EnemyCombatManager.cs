using System;
using System.Collections.Generic;
using ThirdPersonMeleeSystem;
using UnityEngine;

public class EnemyCombatManager : MonoBehaviour
{
    public static EnemyCombatManager Instance;
    public event Action EnemyCountChanged;
    
    [SerializeField]private List<EnemyController> enemiesInCombat = new ();

    [SerializeField] private int currentValue;
    [SerializeField] private int valueLastFrame;

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

    private void Update()
    {
        currentValue = GetEnemyCombatCount();
        
        if (currentValue != valueLastFrame)
        {
            OnEnemyCountChanged();
            Debug.Log("value changed");
        }
            
        valueLastFrame = currentValue;
    }

    public List<EnemyController> GetEnemiesInCombat()
    {
        return enemiesInCombat;
    }

    public void OnEnemyCountChanged()
    {
        EnemyCountChanged?.Invoke();
    }

    public void AddToList(EnemyController enemyToAdd)
    {
        enemiesInCombat.Add(enemyToAdd);
    }
    
    public void RemoveFromList(EnemyController enemyToAdd)
    {
        enemiesInCombat.Remove(enemyToAdd);
    }

    public int GetEnemyCombatCount()
    {
        return enemiesInCombat.Count;
    }
}
