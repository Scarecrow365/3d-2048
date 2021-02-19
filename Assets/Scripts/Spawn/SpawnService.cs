using System.Collections.Generic;
using UnityEngine;

public class SpawnService : MonoBehaviour
{
    [SerializeField] private Transform blockParent;
    [SerializeField] private PoolConfig poolConfig;

    private const int CountLinesOnStart = 4;
    private const float Offset = 1.20f; //cube size (1f) + offset 0.2f

    private PoolService _spawner;

    private void Awake()
    {
        _spawner = gameObject.AddComponent<PoolService>();
        _spawner.Init(poolConfig, blockParent, Offset);
    }

    public GameObject CreateGameObject()
    {
        return _spawner.GetGameObject();
    }

    public List<GameObject> CreateStartCubes()
    {
        var list = new List<GameObject>();
        for (var i = 0; i < CountLinesOnStart; i++)
        {
            list.AddRange(CreateNewLine(list));
        }

        return list;
    }

    public List<GameObject> CreateNewLine(List<GameObject> allCubes)
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

        return _spawner.CreateAndGetNewLine();
    }
}