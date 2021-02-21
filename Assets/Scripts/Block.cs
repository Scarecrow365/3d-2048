using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private Rigidbody _rb;
    private const float Force = 5;

    public int GetBlockScore { get; private set; }

    public event Action<Block> OnTouchSameScoreBlock;
    public event Action<string> OnWinGame;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void SetData(Texture texture, int score)
    {
        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetTexture("_MainTex", texture);
        _renderer.SetPropertyBlock(_propertyBlock);
        GetBlockScore = score;

        if (GetBlockScore >= 2048)
        {
            OnWinGame?.Invoke("win");
        }
    }

    public void AddImpulse()
    {
        var vector = new Vector3(Random.Range(-0.5f, 0.5f), 1f, Random.Range(0, 1f));
        _rb.AddForce(vector * Force, ForceMode.Impulse);
        _rb.rotation = Random.rotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Block"))
        {
            var enemyBlock = other.gameObject.GetComponent<Block>();
            if (enemyBlock.GetBlockScore == GetBlockScore)
            {
                OnTouchSameScoreBlock?.Invoke(this);
                _rb.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }
}