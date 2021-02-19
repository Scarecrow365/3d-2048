using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private SpawnController spawnController;
    [SerializeField] private RedLine[] redLines;

    private int _countMergeCubes;

    private void Start()
    {
        SetUpLevel();
        SubscribeRedLines();
    }

    private void SetUpLevel()
    {
        spawnController.SetUpLevel();
        spawnController.GetCurrentPlayer.OnlaunchPlayer += RequestOnNewPlayer;
        SubscribeBlocks();
    }

    private void SubscribeBlocks()
    {
        foreach (var block in spawnController.AllBlocksInGame)
        {
            SubscribeBlock(block);
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

        if (_countMergeCubes > 1)
        {
            var newBlock = spawnController.RequestOnChildren(block);
            _countMergeCubes = 0;
            SubscribeBlock(newBlock);
        }
        
        UnsubscribeBlock(block);
        spawnController.RemoveDisabledBlockFromList(block);
    }

    private void RequestOnNewPlayer(Player obj)
    {
        spawnController.GetCurrentPlayer.OnlaunchPlayer -= RequestOnNewPlayer;
        spawnController.AddObjectToBlocksList(obj.gameObject);
        StartCoroutine(DelayCreateNewPlayer());
    }

    private void RestartScene(string info)
    {
        print(info);
        SceneManager.LoadScene(0);
    }

    private IEnumerator AddNewBlocksLine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            spawnController.CreateNewLine();
        }
    }

    private IEnumerator DelayCreateNewPlayer()
    {
        yield return new WaitForSeconds(1f);
        spawnController.CreateNewPlayer();
        spawnController.GetCurrentPlayer.OnlaunchPlayer += RequestOnNewPlayer;
    }
}