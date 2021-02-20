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

    public GameObject SpawnFromPool()
    {
        var objectToSpawn = _poolQueue.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = Vector3.zero;
        objectToSpawn.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _poolQueue.Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}