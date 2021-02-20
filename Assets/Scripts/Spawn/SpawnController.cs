using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private BlockData blockData;
    [SerializeField] private Transform blockParent;
    [SerializeField] private PoolConfig poolConfig;
    [SerializeField] private int countLinesOnStart = 5;

    private readonly Vector3 _playerStartPos = new Vector3(0f, 0f, -7f);
    private List<GameObject> _allObjectsInGame;
    private SpawnService _spawnService;

    public List<GameObject> AllBlocksInGame() => _allObjectsInGame;
    public Player GetCurrentPlayer { get; private set; }

    private void Awake()
    {
        _spawnService = gameObject.AddComponent<SpawnService>();
        _spawnService.Init(poolConfig, blockParent);
        _allObjectsInGame = new List<GameObject>();
    }

    public void CreateLevel()
    {
        for (var i = 0; i < countLinesOnStart; i++)
        {
            _allObjectsInGame.AddRange(CreateNewLine());
        }

        CreateNewPlayer();
    }

    public List<GameObject> CreateNewLine()
    {
        var list = _spawnService.CreateNewLine(_allObjectsInGame);

        foreach (var block in list.Select(element => element.GetComponent<Block>()))
        {
            SetDataForBlock(block);
        }

        return list;
    }

    public Block RequestOnChildren(Block parent)
    {
        var block = CreateChildrenBlock();

        foreach (var data in blockData.GetData())
        {
            if (data.score == parent.GetBlockScore * 2)
            {
                block.SetData(data.texture, data.score);
                block.transform.position = parent.transform.position;
                block.AddImpulse();
                break;
            }
        }

        return block;
    }


    public void RemoveDisabledBlockFromList(Block block)
    {
        if (_allObjectsInGame.Any(obj => obj == block.gameObject))
        {
            _allObjectsInGame.Remove(block.gameObject);
        }
    }

    public void CreateNewPlayer()
    {
        var obj = _spawnService.CreateGameObject();
        obj.transform.position = _playerStartPos;
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        obj.tag = "Player";
        obj.layer = LayerMask.NameToLayer("Player");
        var player = obj.GetComponent<Player>();
        player.enabled = true;
        GetCurrentPlayer = player;

        SetDataForBlock(obj.GetComponent<Block>());
    }

    public void AddObjectToObjectsList(GameObject obj)
    {
        _allObjectsInGame.Add(obj);
    }

    private Block CreateChildrenBlock()
    {
        var obj = _spawnService.CreateGameObject();
        _allObjectsInGame.Add(obj);
        var block = obj.GetComponent<Block>();
        return block;
    }

    private void SetDataForBlock(Block block)
    {
        var data = GetRandomData();
        block.SetData(data.texture, data.score);
    }

    private BlockStruct GetRandomData()
    {
        var index = Random.Range(0, 3);
        return blockData.GetData()[index];
    }
}