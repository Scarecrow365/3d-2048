using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private Queue<GameObject> _poolQueue;

    public void Init(PoolConfig poolConfig, Transform parent)
    {
        _poolQueue = new Queue<GameObject>();
        InitDataPool(poolConfig, parent);
    }

    public GameObject SpawnFromPool()
    {
        var objectToSpawn = _poolQueue.Dequeue();
        
        while (objectToSpawn.activeSelf)
        {
            _poolQueue.Enqueue(objectToSpawn);
            objectToSpawn = _poolQueue.Dequeue();
        }

        objectToSpawn.SetActive(true);
        _poolQueue.Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    private void SetDataForWork(Queue<GameObject> queue)
    {
        _poolQueue = queue;
    }

    private void InitDataPool(PoolConfig poolConfig, Transform parent)
    {
        var objects = new Queue<GameObject>();

        foreach (var pool in poolConfig.GetCurrentPool())
        {
            for (var i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, parent);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }

            SetDataForWork(objects);
        }
    }
}