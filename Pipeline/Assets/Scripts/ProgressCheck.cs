using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCheck : MonoBehaviour
{
    public LevelData level;
    private int sizeX, sizeY;
    public Transform pipes;
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"};
    private int[,] mapMatrix;
    private List<Vector2Int> startPosList = new List<Vector2Int>();
    private List<Vector2Int> finishPosList = new List<Vector2Int>();
    public static bool finish = false;

    private void StartCheck()
    {
        //pipes = this.transform;
        sizeX = level.sizeX;
        sizeY = level.sizeY;
        mapMatrix = new int[sizeX, sizeY];
        int count = 0;
        for (int y = sizeY-1; y >= 0; y--)
            for (int x = 0; x < sizeX; x++)
            {
                Debug.Log(pipes.childCount);
                string pipeTag = pipes.GetChild(count).gameObject.tag;
                if (pipeTag == tags[0] || pipeTag == tags[1]) mapMatrix[x, y] = 3;
                else if (pipeTag == tags[2] || pipeTag == tags[3]) mapMatrix[x, y] = 1;
                else mapMatrix[x, y] = -1;

                if (pipeTag == tags[0]) startPosList.Add(new Vector2Int(x,y));
                if (pipeTag == tags[1]) finishPosList.Add(new Vector2Int(x,y));
                count++;
            }

        WriteMartix();
        count = 0;
        for (int y = sizeY-1; y >= 0; y--)
            for (int x = 0; x < sizeX; x++)
            {
                if (mapMatrix[x, y] != -1)
                {
                    string pipeTag = pipes.GetChild(count).gameObject.tag;
                    float rotZ = Mathf.Round(pipes.GetChild(count).eulerAngles.z);
                    Vector2Int vec = new Vector2Int(x,y);
                    CellRecalculation(pipeTag, vec, rotZ);       
                }
                count++;
            }
        WriteMartix();
        CheckWin(null);
    }

    private void OnEnable()
    {
        SpawnBoard.boardSpawned += StartCheck;
        GameController.rotateTube += CheckWin;
    }

    private void CheckWin(GameObject tile)
    {
        if (tile != null)
        {
            int count = 0;
            Vector2Int vec = Vector2Int.zero;
            for (int y = sizeY-1; y >= 0; y--)
                for (int x = 0; x < sizeX; x++)
                {
                    if (tile.transform == pipes.GetChild(count))
                        vec = new Vector2Int(x, y);
                    count++;
                }
            CleanTubes();
            CellRecalculation(tile.tag, vec, Mathf.Round(tile.transform.eulerAngles.z));
            WriteMartix();
        }
        foreach(Vector2Int startPos in startPosList)
            WaterFlow(startPos, startPos);
    }

    private void CellRecalculation(string tag, Vector2Int pos, float rotZ)
    {
        int cellCount = 1;
        if (tag == tags[0] || tag == tags[1])
        {
            cellCount = 3;
            if (rotZ == 0) cellCount += 1;
            if (rotZ == -90 || rotZ == 270) cellCount += 1;
            if (rotZ == -180 || rotZ == 180) cellCount += 1;
            if (rotZ == -270 || rotZ == 90) cellCount += 1;
        }
        else if (tag == tags[3])
        {
            if (rotZ == 0)
            {
                if (mapMatrix[pos.x, pos.y+1] != -1)
                    cellCount += 1;
                if (mapMatrix[pos.x+1, pos.y] != -1)
                    cellCount += 1;
            }
            if (rotZ == -90 || rotZ == 270) 
            {
                if (mapMatrix[pos.x, pos.y+1] != -1)
                    cellCount += 1;
                if (mapMatrix[pos.x, pos.y-1] != -1)
                    cellCount += 1;
            }
            if (rotZ == 180 || rotZ == -180)
            {
                if (mapMatrix[pos.x, pos.y-1] != -1)
                    cellCount += 1;
                if (mapMatrix[pos.x-1, pos.y] != -1)
                    cellCount += 1;
            }
            if (rotZ == -270 || rotZ == 90)
            {
                if (mapMatrix[pos.x, pos.y+1] != -1)
                    cellCount += 1;
                if (mapMatrix[pos.x-1, pos.y] != -1)
                    cellCount += 1;
            }   
        } 
        else if (tag == tags[2])
        {
            if (rotZ == 0 || rotZ == 180 || rotZ == -180)
            {
                if (mapMatrix[pos.x-1, pos.y] != -1)
                    cellCount += 1;
                if (mapMatrix[pos.x+1, pos.y] != -1)
                    cellCount += 1;
            }
            if (rotZ == -270 || rotZ == 270 || rotZ == 90 || rotZ == -90)
            {
                if (mapMatrix[pos.x, pos.y-1] != -1)
                    cellCount += 1;
                if (mapMatrix[pos.x, pos.y+1] != -1)
                    cellCount += 1;
            }          
        }
        if ((tag == tags[2] || tag == tags[3]) && cellCount < 1)
            cellCount = 1;
        mapMatrix[pos.x, pos.y] = cellCount;
    }

    private void WriteMartix()
    {
        string str = "";
        for (int y = sizeY-1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
                str = str + mapMatrix[x, y] + " "; 
            str += "\n";
        } 
        Debug.Log(str);
    }

    private void CleanTubes()
    {
        foreach(Transform child in pipes)
            if (tags.Contains(child.gameObject.tag))
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void WaterFlow(Vector2Int currPos, Vector2Int prevPos)
    {
        int count = 0;
        for (int j = sizeY-1; j >= 0; j--)
        {
            for (int i = 0; i < sizeX; i++)
            {
                if (i == currPos.x && j == currPos.y)
                    pipes.GetChild(count).gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                count++;
            }
                
        } 

        int x = currPos.x;
        int y = currPos.y;

        if (mapMatrix[x, y] == 4) 
        {
            if (startPosList.Contains(currPos)) //start
            {
                if (x == 0 && mapMatrix[x-1, y] == 3)
                    WaterFlow(new Vector2Int(x+1, y), currPos);
                if (x == sizeX-1 && mapMatrix[x-1, y] == 3)
                    WaterFlow(new Vector2Int(x-1, y), currPos);
                if (y == 0 && mapMatrix[x, y+1] == 3)
                    WaterFlow(new Vector2Int(x, y+1), currPos);
                if (y == sizeY-1 && mapMatrix[x, y-1] == 3)
                    WaterFlow(new Vector2Int(x, y-1), currPos);
            }
            if (finishPosList.Contains(currPos)) //finish
            {
                finish = true;
                Debug.Log("finish");
            }
        }
        else if (mapMatrix[x, y] == 3) 
        {
            Vector2Int minusX = new Vector2Int(x-1, y);
            Vector2Int minusY = new Vector2Int(x, y-1);
            Vector2Int plusX = new Vector2Int(x+1, y);
            Vector2Int plusY = new Vector2Int(x, y+1);
            if ((mapMatrix[x-1, y] == 3 && minusX != prevPos) || 
                (mapMatrix[x-1, y] == 4 && finishPosList.Contains(minusX)))
                WaterFlow(new Vector2Int(x-1, y), currPos);
            if ((mapMatrix[x, y+1] == 3 && plusY != prevPos) || 
                (mapMatrix[x, y+1] == 4 && finishPosList.Contains(plusY)))
                WaterFlow(new Vector2Int(x, y+1), currPos);
            if ((mapMatrix[x+1, y] == 3 && plusX != prevPos) || 
                (mapMatrix[x+1, y] == 4 && finishPosList.Contains(plusX)))
                WaterFlow(new Vector2Int(x+1, y), currPos);
            if ((mapMatrix[x, y-1] == 3 && minusY != prevPos) || 
                (mapMatrix[x, y-1] == 4 && finishPosList.Contains(minusY)))
                WaterFlow(new Vector2Int(x, y-1), currPos);
        }
        
    }
}
