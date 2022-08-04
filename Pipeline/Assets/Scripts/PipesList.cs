using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipeData
{
    public string id;
    public int rotZ;
    public GameObject pref;
}

[CreateAssetMenu(fileName = "PipesList", menuName = "Pupeline/Pipes")]
public class PipesList : ScriptableObject 
{
    public List<PipeData> list;
}
