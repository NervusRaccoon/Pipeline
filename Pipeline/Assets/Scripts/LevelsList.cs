using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string id;
    public int sizeX;
    public int sizeY;
    public List<string> content;
}

[CreateAssetMenu(fileName = "LevelsList", menuName = "Pupeline/Levels")]
public class LevelsList : ScriptableObject 
{
    public List<LevelData> list;

    public void AddLevel(LevelData level)
    {
       list.Add(level);
    }
}