using System;
using UnityEngine;

public class RedLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block")) OnTouchRedLine?.Invoke("lose");
    }

    public event Action<string> OnTouchRedLine;
}