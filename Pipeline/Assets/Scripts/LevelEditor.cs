using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class LevelEditor : EditorWindow
{
    public LevelsList savedLevelList;
    public PipesList pipes;
    public GameObject empty;
    private PipeData selectedPipe = null;
    private Vector2 size;
    private PipeData[,] levelList;
    private int sizeX = 0;
    private int sizeY = 0;
    private const string tilePath = "Assets/LevelData";
    private const string prefPath = "Assets/Prefabs";
    private const int itemInLine = 8;
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle", "Empty"};
    private const int cellSize = 60;
    private const int indent = 20;

    [MenuItem("Game/LevelEditor")]
    static void Init()
    {
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        window.Show();
    }

    private void Resize()
    {
        levelList = new PipeData[sizeX, sizeY];
        for (int x = 0; x < sizeY; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (x == 0 && y == 0)
                    levelList[x,y] = pipes.list[11];
                else if (x == 0 && y == sizeY-1)
                    levelList[x,y] = pipes.list[8];
                else if (x == sizeX-1 && y == 0)
                    levelList[x,y] = pipes.list[10];
                else if (x == sizeX-1 && y == sizeY-1)
                    levelList[x,y] = pipes.list[9];
                else if (x == 0)
                    levelList[x,y] = pipes.list[4];
                else if (x == sizeX-1)
                    levelList[x,y] = pipes.list[6];
                else if (y == 0)
                    levelList[x,y] = pipes.list[7];
                else if (y == sizeY-1)
                    levelList[x,y] = pipes.list[5];
                else
                    levelList[x,y] = pipes.list[22];
            }
        }
    }

    private bool Validation()
    {
        bool errorsNotFounded = true;
        int incomingTubesCount = 0;
        int outgoingTubesCount = 0;

        for (int x = 0; x < sizeY; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (x == 0 || y == 0 || x == sizeX-1 || y == sizeY-1)
                {
                    if (levelList[x,y].pref.tag == tags[0])
                        incomingTubesCount++;
                    if (levelList[x,y].pref.tag == tags[1])
                        outgoingTubesCount++;
                    if ((levelList[x,y].pref.tag == tags[2]) || (levelList[x,y].pref.tag == tags[3]))
                        errorsNotFounded = false;
                }
                else
                {
                    if ((levelList[x,y].pref.tag == tags[0]) || (levelList[x,y].pref.tag == tags[1]))
                        errorsNotFounded = false;
                }
            }
        }
        if (incomingTubesCount == 0 || outgoingTubesCount == 0 || incomingTubesCount != outgoingTubesCount)
            errorsNotFounded = false;
        
        return errorsNotFounded;

    }

    void OnGUI()
    {;
        GUILayout.BeginHorizontal();
        size = EditorGUILayout.Vector2Field("Field size ", Vector2Int.RoundToInt(size));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Resize"))
        { 
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            sizeX = (int)size.x;
            sizeY = (int)size.y;
            Resize();
        }
        if (GUILayout.Button("Save"))
        { 
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (Validation())
            {
                LevelData currLevel = new LevelData();
                currLevel.id = savedLevelList.list.Count.ToString();
                currLevel.sizeX = sizeX;
                currLevel.sizeY = sizeY;
                currLevel.content = new List<string>();
                for (int x = 0; x < sizeY; x++)
                    for (int y = 0; y < sizeY; y++)
                        currLevel.content.Add(levelList[x,y].id);
                Debug.Log("Saved");
                savedLevelList.AddLevel(currLevel);
            }
            else
                Debug.Log("Error");
        }  
                
        DrawBoard();

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        int count = 0;
        
        int rectY = indent*2+cellSize;
        if (sizeY != 0)
            rectY = sizeY*cellSize+rectY;
        Rect rect = new Rect(0, rectY, cellSize, cellSize);
        Rect prevRect = rect;
        foreach (PipeData pipe in pipes.list)
        {
            if (count % itemInLine == 0 && count != 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                prevRect = rect;
                rect = new Rect(0, prevRect.y+cellSize, cellSize, cellSize);  
            }
            GUIContent buttonContent = new GUIContent(AssetPreview.GetAssetPreview(pipe.pref));

            Matrix4x4 matrixBackup = GUI.matrix;
            GUIUtility.RotateAroundPivot(-pipe.rotZ, new Vector2(rect.x+cellSize/2, rect.y+cellSize/2));
            if (GUI.Button(rect, buttonContent))
            {
                Debug.Log(pipe.id);
                selectedPipe = pipe;
            }
            prevRect = rect;
            GUI.matrix = matrixBackup;
            rect = new Rect(prevRect.x+cellSize, prevRect.y, cellSize, cellSize);

            count++;  
        }  
    }

    private void DrawBoard()
    {
        Rect rect = new Rect(0, cellSize+indent, cellSize, cellSize);
        Rect prevRect = rect;
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Matrix4x4 matrixBackup = GUI.matrix;
                GUIUtility.RotateAroundPivot(-levelList[x,y].rotZ, new Vector2(rect.x+cellSize/2, rect.y+cellSize/2));

                var buttonContent = new GUIContent(AssetPreview.GetAssetPreview(levelList[x,y].pref));
                if (GUI.Button(rect, buttonContent))
                {
                    if (selectedPipe != null)
                        levelList[x,y] = selectedPipe;
                } 
                prevRect = rect;
                GUI.matrix = matrixBackup;
                rect = new Rect(prevRect.x+cellSize, prevRect.y, cellSize, cellSize);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            rect = new Rect(0, prevRect.y+cellSize, cellSize, cellSize); 
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
    }
}