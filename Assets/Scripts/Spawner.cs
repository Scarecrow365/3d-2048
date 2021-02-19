using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform blockParent;
    [SerializeField] private PoolConfig poolConfig;
    
    private ObjectPooler _pool;
    private const float Offset = 1.20f; //cube size (1f) + offset 0.2f
    private const int CountLinesOnStart = 4;
    private const int CubesInRow = 5;
    private readonly Vector3 _startPos = new Vector3(-2.40f, 0f, 6.80f); //left corner
    private readonly Vector3 _startPlayerPos = new Vector3(0f, 0f, -7f);
    private List<GameObject> _objectList;

    public List<GameObject> GetObjectList() => _objectList;

    public void Init()
    {
        _objectList = new List<GameObject>();
        _pool = new ObjectPooler();
        _pool.Init();
        
        InitDataPool();
    }

    public void StartPackBlocks()
    {
        var currentPos = _startPos;

        for (int i = 0; i < CountLinesOnStart; i++)
        {
            currentPos = i == 0
                ? new Vector3(currentPos.x, currentPos.y, currentPos.z)
                : new Vector3(currentPos.x, currentPos.y, currentPos.z - Offset);
            var list = CreateNewLine(currentPos);
            _objectList.AddRange(list);
        }
    }

    public Player CreatePlayer()
    {
        var obj = _pool.SpawnFromPool();
        obj.transform.position = _startPlayerPos;
        _objectList.Add(obj);
        obj.tag = "Player";
        obj.layer = LayerMask.NameToLayer("Player");
        obj.GetComponent<Block>().enabled = false;
        var player = obj.GetComponent<Player>();
        player.enabled = true;
        return player;
    }

    public void RemoveObjectFromList(GameObject obj)
    {
        if (_objectList.Contains(obj))
        {
            _objectList.Remove(obj);
        }
    }

    public List<GameObject> RequestOnNewLine()
    {
        foreach (var obj in _objectList)
        {
            obj.transform.position = new Vector3(
                obj.transform.position.x,
                obj.transform.position.y,
                obj.transform.position.z - Offset);
        }

        var rowList = CreateNewLine(_startPos);
        _objectList.AddRange(rowList);
        return rowList;
    }

    private List<GameObject> CreateNewLine(Vector3 currentPos)
    {
        var objectsList = new List<GameObject>();
        for (var i = 0; i < CubesInRow; i++)
        {
            if (i == 0)
            {
                var block1 = _pool.SpawnFromPool();
                block1.transform.position = currentPos;
                objectsList.Add(block1);
                continue;
            }

            var block = _pool.SpawnFromPool();
            block.transform.position = new Vector3(currentPos.x + Offset, currentPos.y, currentPos.z);
            currentPos = block.transform.position;
            objectsList.Add(block);
        }

        return objectsList;
    }

    private void InitDataPool()
    {
        var objects = new Queue<GameObject>();

        foreach (var pool in poolConfig.GetCurrentPool())
        {
            for (var i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, blockParent);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }

            _pool.SetDataForWork(objects);
        }
    }
}