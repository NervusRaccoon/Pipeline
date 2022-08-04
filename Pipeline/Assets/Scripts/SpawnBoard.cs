using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoard : MonoBehaviour
{
    public LevelData level;
    public PipesList pipesList;
    public GameObject empty;
    public Transform pipes;
    //public Transform levelTransform;
    private Transform board;
    private Quaternion[] dir = new Quaternion[] {Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, -180), Quaternion.Euler(0, 0, -270)};
    private const string flatTag = "Flat";
    private const string angleTag = "Angle";

    private void Start()
    {
        board = this.transform;
        SpawnMap();
        SpawnPipes();
        Randomize();
    }

    private void Randomize()
    {
        foreach(Transform child in pipes)
            if (child.gameObject.tag == angleTag || child.gameObject.tag == flatTag)
                child.rotation = dir[Random.Range(0, dir.Length-1)];
    }

    private void SpawnMap()
    {
        int xSize = level.sizeX;
        int ySize = level.sizeY;
        GameObject[,] tileArr = new GameObject[xSize, ySize];
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = empty.GetComponent<SpriteRenderer>().bounds.size;

        int count = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(empty, transform.position, Quaternion.identity);
                newTile.transform.position = new Vector3(xPos + (tileSize.x*x), yPos + (tileSize.y*y), 0);
                newTile.transform.parent = transform;
                newTile.name = count.ToString();
                count++;

                tileArr[x, y] = newTile;
            }            
        }
    }

    private void SpawnPipes()
    {
        int xSize = level.sizeX;
        int ySize = level.sizeY;
        GameObject[,] tileArr = new GameObject[xSize, ySize];
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
                    if (pipe.id == level.content[count])
                    {
                        count++;
                        pref = pipe.pref;
                        pref.name = pipe.id;
                        pref.transform.rotation = Quaternion.Euler(0, 0, pipe.rotZ);
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
