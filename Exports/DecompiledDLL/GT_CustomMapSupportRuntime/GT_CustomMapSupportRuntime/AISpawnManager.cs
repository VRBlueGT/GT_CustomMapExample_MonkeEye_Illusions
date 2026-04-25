using System.Collections.Generic;
using UnityEngine;

namespace GT_CustomMapSupportRuntime;

public class AISpawnManager : MonoBehaviour
{
    private Dictionary<int, GameObject> enemyTypes = new Dictionary<int, GameObject>(64);

    public static AISpawnManager? instance;

    private static bool hasInstance;

    private Dictionary<string, AISpawnPoint> spawnPoints = new Dictionary<string, AISpawnPoint>(128);

    public static bool HasInstance => hasInstance;

    private void Awake()
    {
        if ((Object)(object)instance != (Object)null)
        {
            Object.Destroy((Object)(object)this);
            return;
        }
        instance = this;
        hasInstance = true;
        GetEnemyTypeTemplates();
        FindSpawnPoints();
    }

    public void FindSpawnPoints()
    {
        spawnPoints.Clear();
        AISpawnPoint[] array = Object.FindObjectsByType<AISpawnPoint>((FindObjectsInactive)0, (FindObjectsSortMode)0);
        for (int i = 0; i < array.Length; i++)
        {
            spawnPoints.Add(array[i].spawnID, array[i]);
        }
    }

    public bool GetSpawnPoint(string spawnPointID, out AISpawnPoint spawnPoint)
    {
        if (!spawnPoints.TryGetValue(spawnPointID, out spawnPoint))
        {
            return false;
        }
        return true;
    }

    public bool GetEnemyType(int enemyTypeIndex, out GameObject? newEnemy)
    {
        if (!enemyTypes.ContainsKey(enemyTypeIndex))
        {
            newEnemy = null;
            return false;
        }
        newEnemy = enemyTypes[enemyTypeIndex];
        return true;
    }

    public bool SpawnEnemy(string spawnPointID, int enemyTypeIndex, out AIAgent? newEnemy)
    {
        if (!enemyTypes.ContainsKey(enemyTypeIndex))
        {
            Debug.Log((object)"AISpawnManager::SpawnEnemy enemy index incorrect");
            newEnemy = null;
            return false;
        }
        if (!spawnPoints.TryGetValue(spawnPointID, out AISpawnPoint value))
        {
            Debug.Log((object)"AISpawnManager::SpawnEnemy Can't find spawn point");
            newEnemy = null;
            return false;
        }
        GameObject val = Object.Instantiate<GameObject>(enemyTypes[enemyTypeIndex], ((Component)value).transform);
        value.spawnCount++;
        newEnemy = val.GetComponent<AIAgent>();
        return true;
    }

    public bool SpawnEnemy(int enemyTypeIndex, out AIAgent? newEnemy)
    {
        if (!enemyTypes.ContainsKey(enemyTypeIndex))
        {
            Debug.Log((object)"AISpawnManager::SpawnEnemy enemy index incorrect");
            newEnemy = null;
            return false;
        }
        GameObject val = Object.Instantiate<GameObject>(enemyTypes[enemyTypeIndex]);
        newEnemy = val.GetComponent<AIAgent>();
        return true;
    }

    private void GetEnemyTypeTemplates()
    {
        for (int i = 0; i < ((Component)this).transform.childCount; i++)
        {
            Transform child = ((Component)this).transform.GetChild(i);
            AIAgent component = ((Component)child).GetComponent<AIAgent>();
            if (!((Object)(object)component == (Object)null) && component.isTemplate)
            {
                ((Component)child).gameObject.SetActive(false);
                enemyTypes[component.enemyTypeId] = ((Component)child).gameObject;
            }
        }
    }
}
