using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TilePos
{
    public Vector2 pos;
    public int rotZ;
    public TileData tile;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Pupeline/Levels")]
public class LevelData : ScriptableObject 
{
    public string name;
    public int sizeX;
    public int sizeY;
    public List<TilePos> content;
}