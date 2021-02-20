using System.Collections.Generic;
using UnityEngine;

public class SpawnService : MonoBehaviour
{
    private const int CubesInRow = 5;
    private const float Offset = 1.20f; //cube size (1f) + offset 0.2f
    private readonly Vector3 _startPos = new Vector3(-2.40f, 0f, 6.80f); //left corner

    private ObjectPooler _pool;

    public void Init(PoolConfig poolConfig,Transform blockParent)
    {
        _pool = gameObject.AddComponent<ObjectPooler>();
        _pool.Init(poolConfig, blockParent);
    }

    public GameObject CreateGameObject()
    {
        return _pool.SpawnFromPool();
    }

    public List<GameObject> CreateNewLine(List<GameObject> allCubes)
    {
        MoveAllCubesForNewLine(allCubes);
        return CreateAndGetNewLine();
    }
    
    private List<GameObject> CreateAndGetNewLine()
    {
        var objectsList = new List<GameObject>();
        var currentPos = _startPos;

        for (var i = 0; i < CubesInRow; i++)
        {
            var block = CreateGameObject();
            block.transform.position =
                i == 0 ? _startPos : new Vector3(currentPos.x + Offset, currentPos.y, currentPos.z);
            currentPos = block.transform.position;
            objectsList.Add(block);
        }

        return objectsList;
    }

    private void MoveAllCubesForNewLine(List<GameObject> allCubes)
    {
        foreach (var obj in allCubes)
        {
            var position = obj.transform.position;
            position = new Vector3(
                position.x,
                position.y,
                position.z - Offset);
            obj.transform.position = position;
        }
    }
}