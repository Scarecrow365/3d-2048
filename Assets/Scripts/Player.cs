using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    private Camera _camera;
    private Player _player;
    private Block _block;
    private const int Force = 30;

    public Action<Player> OnlaunchPlayer;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
        _block = GetComponent<Block>();
        _camera = Camera.main;
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 2.6f; //distance from block zero pos to right border;
        transform.position = new Vector3(
            _camera.ScreenToWorldPoint(mousePos).x, 
            transform.position.y,
            transform.position.z);

        if (Input.GetMouseButtonUp(0))
        {
            _rb.velocity = Vector3.forward * Force;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RedLine"))
        {
            OnlaunchPlayer?.Invoke(this);
            gameObject.tag = "Block";
            gameObject.layer = LayerMask.NameToLayer("Default");
            _block.enabled = true;
            _player.enabled = false;
        }
    }
}