using System.Collections.Generic;
using UnityEngine;

public class PoolService : MonoBehaviour
{
    private const int CubesInRow = 5;
    private readonly Vector3 _startPos = new Vector3(-2.40f, 0f, 6.80f); //left corner

    private float _offset;
    private ObjectPooler _pool;

    public void Init(PoolConfig poolConfig, Transform parent, float offset)
    {
        _pool = new ObjectPooler();
        _pool.Init();
        _offset = offset;

        InitDataPool(poolConfig, parent);
    }

    public GameObject GetGameObject()
    {
        return _pool.SpawnFromPool();
    }

    public List<GameObject> CreateAndGetNewLine()
    {
        var objectsList = new List<GameObject>();
        var currentPos = _startPos;

        for (var i = 0; i < CubesInRow; i++)
        {
            var block = _pool.SpawnFromPool();
            block.transform.position =
                i == 0 ? _startPos : new Vector3(currentPos.x + _offset, currentPos.y, currentPos.z);
            currentPos = block.transform.position;
            objectsList.Add(block);
        }

        return objectsList;
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

            _pool.SetDataForWork(objects);
        }
    }
}