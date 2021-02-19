using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler
{
    private Queue<GameObject> _poolQueue;

    public void Init()
    {
        _poolQueue = new Queue<GameObject>();
    }

    public void SetDataForWork(Queue<GameObject> queue)
    {
        _poolQueue = queue;
    }

    public GameObject SpawnFromPool()
    {
        var objectToSpawn = _poolQueue.Dequeue();
        objectToSpawn.SetActive(true);
        _poolQueue.Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}