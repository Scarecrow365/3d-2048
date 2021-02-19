using System;
using UnityEngine;

public class RedLine : MonoBehaviour
{
    public event Action<string> OnTouchRedLine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            OnTouchRedLine?.Invoke("lose");
        }
    }
}