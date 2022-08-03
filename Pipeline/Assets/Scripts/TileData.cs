using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Pupeline/Tiles")]
public class TileData : ScriptableObject 
{
    public string name;
    public Sprite img;
    public string tag;
}
