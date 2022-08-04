using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Pupeline/Levels")]
public class LevelData : ScriptableObject 
{
    public string id;
    public int sizeX;
    public int sizeY;
    public List<string> content;
}

