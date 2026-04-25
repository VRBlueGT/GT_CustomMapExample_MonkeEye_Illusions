using System.Collections.Generic;
using UnityEngine;

namespace GT_CustomMapSupportRuntime;

public class MapSpawnManager : MonoBehaviour
{
    private Dictionary<int, GameObject> entityTypes = new Dictionary<int, GameObject>(64);

    public static MapSpawnManager? instance;

    private static bool hasInstance;

    private Dictionary<string, MapSpawnPoint> spawnPoints = new Dictionary<string, MapSpawnPoint>(128);

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
        GetEntityTypeTemplates();
        FindSpawnPoints();
    }

    public void FindSpawnPoints()
    {
        spawnPoints.Clear();
        MapSpawnPoint[] array = Object.FindObjectsByType<MapSpawnPoint>((FindObjectsInactive)0, (FindObjectsSortMode)0);
        for (int i = 0; i < array.Length; i++)
        {
            spawnPoints.Add(array[i].spawnID, array[i]);
        }
    }

    public bool GetSpawnPoint(string spawnPointID, out MapSpawnPoint spawnPoint)
    {
        if (!spawnPoints.TryGetValue(spawnPointID, out spawnPoint))
        {
            return false;
        }
        return true;
    }

    public bool GetEntityType(int enemyTypeIndex, out GameObject? newEntity)
    {
        if (!entityTypes.ContainsKey(enemyTypeIndex))
        {
            newEntity = null;
            return false;
        }
        newEntity = entityTypes[enemyTypeIndex];
        return true;
    }

    public bool SpawnEntity(string spawnPointID, int enemyTypeIndex, out MapEntity? newEntity)
    {
        if (!entityTypes.ContainsKey(enemyTypeIndex))
        {
            Debug.Log((object)"AISpawnManager::SpawnEnemy enemy index incorrect");
            newEntity = null;
            return false;
        }
        if (!spawnPoints.TryGetValue(spawnPointID, out MapSpawnPoint value))
        {
            Debug.Log((object)"AISpawnManager::SpawnEnemy Can't find spawn point");
            newEntity = null;
            return false;
        }
        GameObject val = Object.Instantiate<GameObject>(entityTypes[enemyTypeIndex], ((Component)value).transform);
        value.spawnCount++;
        newEntity = val.GetComponent<MapEntity>();
        return true;
    }

    public bool SpawnEntity(int enemyTypeIndex, out MapEntity? newEnemy)
    {
        if (!entityTypes.ContainsKey(enemyTypeIndex))
        {
            Debug.Log((object)"AISpawnManager::SpawnEnemy enemy index incorrect");
            newEnemy = null;
            return false;
        }
        GameObject val = Object.Instantiate<GameObject>(entityTypes[enemyTypeIndex]);
        newEnemy = val.GetComponent<MapEntity>();
        return true;
    }

    private void GetEntityTypeTemplates()
    {
        for (int i = 0; i < ((Component)this).transform.childCount; i++)
        {
            Transform child = ((Component)this).transform.GetChild(i);
            MapEntity component = ((Component)child).GetComponent<MapEntity>();
            if (!((Object)(object)component == (Object)null) && component.isTemplate)
            {
                ((Component)child).gameObject.SetActive(false);
                entityTypes[component.entityTypeId] = ((Component)child).gameObject;
            }
        }
    }
}
