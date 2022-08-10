using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResetButton : EditorWindow
{
    [MenuItem("Game/Reset")]
    static void Init()
    {
        PlayerPrefs.SetInt("levelNumber", 0);
    }
}