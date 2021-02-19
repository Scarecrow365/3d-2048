using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Block Data", fileName = "BlockData", order = 0)]
public class BlockData : ScriptableObject
{
    [SerializeField] private List<BlockStruct> dataStruct;
    public List<BlockStruct> GetData() => dataStruct;
}

[Serializable]
public struct BlockStruct
{
    public int score;
    public Texture texture;
}