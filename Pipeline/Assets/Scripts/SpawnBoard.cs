using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoard : MonoBehaviour
{
    private int xSize, ySize;
    public LevelsList levelsDeta;
    public int levelNumber;
    public PipesList pipesList;
    public GameObject empty;
    public Transform pipes;
    public ProgressCheck progressCheck;
    private Transform board;
    private Quaternion[] dir = new Quaternion[] {Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, -180), Quaternion.Euler(0, 0, -270)};
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"};

    private void Start()
    {
        board = this.transform;
        StartSpawnBoard();
    }

    public void StartSpawnBoard()
    {
        levelNumber = PlayerPrefs.GetInt("levelNumber");
        if (levelNumber == null)
        {
            PlayerPrefs.SetInt("levelNumber", 1);
            levelNumber = 1;
        }
        if (levelNumber > levelsDeta.list.Count-1)
            levelNumber = levelsDeta.list.Count-1;

        CleanBoard();
        SpawnMap();
        SpawnPipes();
        Randomize();
        progressCheck.StartCheck(xSize, ySize);
    }

    private void CleanBoard()
    {
        while (board.childCount > 0) DestroyImmediate(board.GetChild(0).gameObject);
        while (pipes.childCount > 0) DestroyImmediate(pipes.GetChild(0).gameObject);
    }

    public void Randomize()
    {
        foreach(Transform child in pipes)
            if (child.gameObject.tag == tags[2] || child.gameObject.tag == tags[3])
                child.rotation = dir[Random.Range(0, dir.Length-1)];
    }

    private void SpawnMap()
    {
        xSize = levelsDeta.list[levelNumber].sizeX;
        ySize = levelsDeta.list[levelNumber].sizeY;
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = empty.GetComponent<SpriteRenderer>().bounds.size;

        int count = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(empty, transform.position, Quaternion.identity, board);
                newTile.transform.position = new Vector3(xPos + (tileSize.x*x), yPos + (tileSize.y*y), 0);
                newTile.name = count.ToString();
                count++;
            }            
        }
        board.position = new Vector3((-1)*tileSize.x*xSize/2 + tileSize.x/2, (-1)*tileSize.y*ySize/2 + tileSize.y/2, 0);
        pipes.position = new Vector3((-1)*tileSize.x*xSize/2 + tileSize.x/2, (-1)*tileSize.y*ySize/2 + tileSize.y/2, 0);
    }

    public void SpawnPipes()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = empty.GetComponent<SpriteRenderer>().bounds.size;
        int count = 0;

        for (int y = ySize-1; y >= 0; y--)
        {
            for (int x = 0; x < xSize; x++)
            {
                GameObject pref = null;
                foreach(PipeData pipe in pipesList.list)
                {
                    if (pipe.id == levelsDeta.list[levelNumber].content[count])
                    {
                        pref = pipe.pref;
                        pref.name = pipe.id;
                        pref.transform.rotation = Quaternion.Euler(0, 0, pipe.rotZ);
                        count++;
                        break;
                    }
                }
                if (pref != null)
                {
                    GameObject newTile = Instantiate(pref, transform.position, pref.transform.rotation, pipes);
                    newTile.transform.position = new Vector3(xPos + (tileSize.x*x), yPos + (tileSize.y*y), 0);
                    newTile.name = pref.name;
                }
            }            
        }      
    }

}
