using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbors
{
    public bool active = false;
    public Vector2Int cellPos;
    public List<Vector2Int> neighborPosition;
    public List<int> neighborLink;
}

public class ProgressCheck : MonoBehaviour
{
    public LevelData level;
    private int sizeX, sizeY;
    public Transform pipes;
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"};
    private int[,] mapMatrix;
    private List<Vector2Int> startPosList = new List<Vector2Int>();
    private List<Vector2Int> finishPosList = new List<Vector2Int>();
    private List<Vector2Int> neighborConstList = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(-1,0), new Vector2Int(0,1), new Vector2Int(0,-1)};
    public static bool finish = false;
    private Neighbors[,] neighborsList;

    private void StartCheck()
    {
        //pipes = this.transform;
        sizeX = level.sizeX;
        sizeY = level.sizeY;
        mapMatrix = new int[sizeX, sizeY];
        neighborsList = new Neighbors[sizeX, sizeY];
        int count = 0;
        for (int y = sizeY-1; y >= 0; y--)
            for (int x = 0; x < sizeX; x++)
            {
                Vector2Int pos = new Vector2Int(x,y);
                Neighbors currNeihgbors = new Neighbors();
                currNeihgbors.cellPos = pos;
                currNeihgbors.neighborPosition = new List<Vector2Int>();
                foreach (Vector2Int constN in neighborConstList)
                    currNeihgbors.neighborPosition.Add(constN + pos);
                currNeihgbors.neighborLink = new List<int>{0,0,0,0};
                
                string pipeTag = pipes.GetChild(count).gameObject.tag;

                if (pipeTag == tags[0]) startPosList.Add(pos);
                if (pipeTag == tags[1]) finishPosList.Add(pos);
                if (tags.Contains(pipeTag)) currNeihgbors.active = true;
                neighborsList[x,y] = currNeihgbors;

                count++;
            }


        count = 0;
        for (int y = sizeY-1; y >= 0; y--)
            for (int x = 0; x < sizeX; x++)
            {
                string pipeTag = pipes.GetChild(count).gameObject.tag;
                float currRotationZ = Mathf.Round(pipes.GetChild(count).eulerAngles.z);
                Vector2Int vec = new Vector2Int(x,y);
                CellRecalculation(pipeTag, vec, currRotationZ, currRotationZ);       
                count++;
            }
        WriteMartix();

        CheckWin(null, 0);
    }

    private void OnEnable()
    {
        SpawnBoard.boardSpawned += StartCheck;
        GameController.rotateTube += CheckWin;
    }

    private void CheckWin(GameObject tile, float prevRotationZ)
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
            CellRecalculation(tile.tag, vec, Mathf.Round(tile.transform.eulerAngles.z), Mathf.Round(prevRotationZ));
            foreach(Vector2Int startPos in startPosList)
                WaterFlow(startPos, startPos);
        }
        WriteMartix();
    }

    private void CellRecalculation(string tag, Vector2Int pos, float currRotationZ, float prevRotationZ)
    { 
        if (neighborsList[pos.x, pos.y].active)
            if (tag == tags[0] || tag == tags[1])
            {
                if (currRotationZ == 0) neighborsList[pos.x, pos.y].neighborLink[0] = 1;
                if (currRotationZ == -90 || currRotationZ == 270) neighborsList[pos.x, pos.y].neighborLink[3] = 1;
                if (currRotationZ == -180 || currRotationZ == 180) neighborsList[pos.x, pos.y].neighborLink[1] = 1;
                if (currRotationZ == -270 || currRotationZ == 90) neighborsList[pos.x, pos.y].neighborLink[2] = 1;
            }
            else if (tag == tags[3] || tag == tags[2])
            {
                AngleConversion(tag, currRotationZ, 1, pos);
                Debug.Log(currRotationZ + " ? " + prevRotationZ);
                if (currRotationZ != prevRotationZ)
                {
                    Debug.Log("Не равны");
                    AngleConversion(tag, prevRotationZ, -1, pos); 
                }
            }
    }

    private void AngleConversion(string tag, float angle, int count, Vector2Int pos)
    {
        if (tag == tags[3])
        {
            if (angle == 0)
            {
                neighborsList[pos.x, pos.y].neighborLink[0] += count;
                neighborsList[pos.x, pos.y].neighborLink[2] += count;
            }
            if (angle == -90 || angle == 270) 
            {
                neighborsList[pos.x, pos.y].neighborLink[0] += count;
                neighborsList[pos.x, pos.y].neighborLink[3] += count;
            }
            if (angle == 180 || angle == -180)
            {
                neighborsList[pos.x, pos.y].neighborLink[1] += count;
                neighborsList[pos.x, pos.y].neighborLink[3] += count;
            }
            if (angle == -270 || angle == 90)
            {
                neighborsList[pos.x, pos.y].neighborLink[1] += count;
                neighborsList[pos.x, pos.y].neighborLink[2] += count;
            } 
        } else if (tag == tags[2])
        {
            if (angle == 0 || angle == 180 || angle == -180)
            {
                neighborsList[pos.x, pos.y].neighborLink[0] += count;
                neighborsList[pos.x, pos.y].neighborLink[1] += count;
            }
            if (angle == -270 || angle == 270 || angle == 90 || angle == -90)
            {
                neighborsList[pos.x, pos.y].neighborLink[2] += count;
                neighborsList[pos.x, pos.y].neighborLink[3] += count;
            }
        }  
    }

    private void WriteMartix()
    {
        string str = "";
        for (int y = sizeY-1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (neighborsList[x,y].active)
                {
                    str = str + neighborsList[x,y].cellPos.ToString() + " : ";
                    
                    for (int z = 0; z < 4; z++)
                        str = str + neighborsList[x,y].neighborLink[z].ToString() + " "; 
                    str += "\n";
                }
            }
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
        int nextID = -1;
        int prevID = -1;
        Neighbors cell = neighborsList[currPos.x, currPos.y];
        if (cell.active)
        {
            int count = 0;
            for (int y = sizeY-1; y >= 0; y--)
                for (int x = 0; x < sizeX; x++)
                {
                    if (prevPos.x == x && prevPos.y == y)
                        pipes.GetChild(count).gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    count++;
                }
            for (int i = 0; i < 4; i++)
            {
                if (cell.neighborLink[i] == 1)
                    if (cell.neighborPosition[i] != prevPos)
                        nextID = i;
                    else
                        prevID = i;
            }
            if (currPos == prevPos) WaterFlow(cell.neighborPosition[nextID], currPos);
            if (prevID != -1 && nextID != -1) WaterFlow(cell.neighborPosition[nextID], currPos);
            else if (prevID != -1 && nextID == -1) 
            {
                count = 0;
                for (int y = sizeY-1; y >= 0; y--)
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (currPos.x == x && currPos.y == y)
                            pipes.GetChild(count).gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                        count++;
                    }
                finish = true;
            }
        }    
    }
}
