using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const int Force = 30;
    private Block _block;
    private Camera _camera;
    private Player _player;
    private Rigidbody _rb;

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
        if (Input.GetMouseButton(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 15f))
            {
                transform.position = new Vector3(hit.point.x, 0f, transform.position.z);
            }
        }

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