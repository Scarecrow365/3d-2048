using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private SpawnService spawnService;
    [SerializeField] private BlockData blockData;

    private readonly Vector3 _playerStartPos = new Vector3(0f, 0f, -7f);
    private List<GameObject> _allObjectsInGame;

    public List<Block> AllBlocksInGame { get; private set; }
    public Player GetCurrentPlayer { get; private set; }

    public void SetUpLevel()
    {
        AllBlocksInGame = new List<Block>(55);
        _allObjectsInGame = spawnService.CreateStartCubes();

        SetDataForAllCubes();
        CreateNewPlayer();
    }

    public void CreateNewLine()
    {
        spawnService.CreateNewLine(_allObjectsInGame);
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

    private Block CreateChildrenBlock()
    {
        var obj = spawnService.CreateGameObject();
        _allObjectsInGame.Add(obj);
        var block = obj.GetComponent<Block>();
        AllBlocksInGame.Add(block);
        return block;
    }

    public void RemoveDisabledBlockFromList(Block block)
    {
        Block element = new Block();
        
        foreach (var obj in _allObjectsInGame)
        {
            if (obj == block.gameObject)
            {
                element = block;
            }
        }
        
        _allObjectsInGame.Remove(element.gameObject);
        AllBlocksInGame.Remove(element);
    }

    private void SetDataForAllCubes()
    {
        foreach (var obj in _allObjectsInGame)
        {
            SetDataForBlock(obj.GetComponent<Block>());
        }
    }

    private void SetDataForBlock(Block block)
    {
        AllBlocksInGame.Add(block);
        var data = GetRandomData();
        block.SetData(data.texture, data.score);
    }

    public void CreateNewPlayer()
    {
        var obj = spawnService.CreateGameObject();
        obj.transform.position = _playerStartPos;

        obj.tag = "Player";
        obj.layer = LayerMask.NameToLayer("Player");
        var player = obj.GetComponent<Player>();
        player.enabled = true;
        GetCurrentPlayer = player;

        SetDataForBlock(obj.GetComponent<Block>());
    }

    public void AddObjectToBlocksList(GameObject obj)
    {
        _allObjectsInGame.Add(obj);
    }

    private BlockStruct GetRandomData()
    {
        var index = Random.Range(0, 3);
        return blockData.GetData()[index];
    }
}