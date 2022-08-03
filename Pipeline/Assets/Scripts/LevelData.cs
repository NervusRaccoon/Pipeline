using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Pupeline/Levels")]
public class LevelData : ScriptableObject 
{
    public string name;
    public int size;
    public GameObject content;
}