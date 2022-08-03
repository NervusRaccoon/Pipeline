using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class LevelEditor : EditorWindow
{
    private string sizeX, sizeY;
    private const string tilePath = "Assets/LevelData";
    private const string prefPath = "Assets/Prefabs";
    public Sprite empty;

    [MenuItem("Game/LevelEditor")]
    static void Init()
    {
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        window.Show();
    }

    void OnGUI()
    {;
        GUILayout.Label("Field size ", EditorStyles.boldLabel);
        sizeX = EditorGUILayout.TextField("X: ", sizeX);
        sizeY = EditorGUILayout.TextField("Y: ", sizeY);
        EditorGUILayout.BeginHorizontal();
        //empty = (Sprite)EditorGUILayout.ObjectField(empty, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        empty = (Sprite)EditorGUILayout.ObjectField(empty, typeof(Sprite), allowSceneObjects: true);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Resize"))
        {
            int x = Int32.Parse(sizeX);
            int y = Int32.Parse(sizeY);

            Resize(x, y);
        }

    }

    private void Resize(int sizeX, int sizeY)
    {
        Debug.Log("where");
       /* GameObject[,] tileArr = new GameObject[sizeX, sizeY];
        float xPos = empty.transform.position.x;
        float yPos = empty.transform.position.y;
        Vector2 tileSize = empty.GetComponent<SpriteRenderer>().bounds.size;

        int count = 0;
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                GameObject newTile = Instantiate(empty, empty.transform.position, Quaternion.identity);
                newTile.transform.position = new Vector3(xPos + (tileSize.x*x), yPos + (tileSize.y*y), 0);
                //newTile.transform.parent = empty.transform;
                newTile.name = count.ToString();
                count++;

                tileArr[x, y] = newTile;
            }            
        }*/
    }
}