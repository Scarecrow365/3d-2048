using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "Scriptable Objects/PoolConfig", order = 0)]
public class PoolConfig : ScriptableObject
{
    [SerializeField] private List<Pool> pools;

    public List<Pool> GetCurrentPool()
    {
        return pools;
    }

    [Serializable]
    public struct Pool
    {
        public GameObject prefab;
        public int size;
    }
}