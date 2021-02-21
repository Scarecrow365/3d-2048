using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private SpawnController spawnController;
    [SerializeField] private RedLine[] redLines;

    private int _countMergeCubes;
    private int _countPlayersLaunch;

    private void Start()
    {
        SetUpLevel();
        SubscribeRedLines();
        //StartCoroutine(AddNewBlocksLine());
    }

    private void SetUpLevel()
    {
        spawnController.CreateLevel();
        var player = spawnController.GetCurrentPlayer;
        player.OnlaunchPlayer += RequestOnNewPlayer;
        SubscribeBlock(player.gameObject.GetComponent<Block>());
        SubscribeBlocks();
    }

    private void SubscribeBlocks()
    {
        foreach (var block in spawnController.AllBlocksInGame())
        {
            SubscribeBlock(block.GetComponent<Block>());
        }
    }

    private void SubscribeBlock(Block block)
    {
        block.OnWinGame += RestartScene;
        block.OnTouchSameScoreBlock += RequestOnChild;
    }

    private void UnsubscribeBlock(Block block)
    {
        block.OnWinGame -= RestartScene;
        block.OnTouchSameScoreBlock -= RequestOnChild;
    }

    private void SubscribeRedLines()
    {
        foreach (var line in redLines)
        {
            line.OnTouchRedLine += RestartScene;
        }
    }

    private void RequestOnChild(Block block)
    {
        _countMergeCubes++;

        UnsubscribeBlock(block);
        spawnController.RemoveDisabledBlockFromList(block);

        if (_countMergeCubes > 1)
        {
            var newBlock = spawnController.RequestOnChildren(block);
            _countMergeCubes = 0;
            SubscribeBlock(newBlock);
        }
    }

    private void RequestOnNewPlayer(Player parent)
    {
        _countPlayersLaunch++;
        
        parent.OnlaunchPlayer -= RequestOnNewPlayer;
        spawnController.AddObjectToObjectsOnFieldList(parent.gameObject);
        StartCoroutine(DelayCreateNewPlayer());
    }

    private void RestartScene(string info)
    {
        Debug.Log($"How much player launch cubes: {_countPlayersLaunch}");
        SceneManager.LoadScene(0);
    }

    private IEnumerator AddNewBlocksLine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            var newLine = spawnController.CreateNewLine();

            foreach (var block in newLine)
            {
                SubscribeBlock(block.GetComponent<Block>());
            }
        }
    }

    private IEnumerator DelayCreateNewPlayer()
    {
        yield return new WaitForSeconds(1f);
        spawnController.CreateNewPlayer();
        var player = spawnController.GetCurrentPlayer;
        player.OnlaunchPlayer += RequestOnNewPlayer;
        SubscribeBlock(player.GetComponent<Block>());
    }
}