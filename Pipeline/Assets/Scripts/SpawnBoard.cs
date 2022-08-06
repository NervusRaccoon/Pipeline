using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoard : MonoBehaviour
{
    public LevelData level;
    public PipesList pipesList;
    public GameObject empty;
    public Transform pipes;
    private Quaternion[] dir = new Quaternion[] {Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, -180), Quaternion.Euler(0, 0, -270)};
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"};

    public static System.Action boardSpawned;


    private void Start()
    {
        SpawnMap();
        SpawnPipes();
        Randomize();
    }

    private void Randomize()
    {
        foreach(Transform child in pipes)
            if (child.gameObject.tag == tags[2] || child.gameObject.tag == tags[3])
                child.rotation = dir[Random.Range(0, dir.Length-1)];
        boardSpawned?.Invoke();
    }

    private void SpawnMap()
    {
        int xSize = level.sizeX;
        int ySize = level.sizeY;
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
            }            
        }
    }

    public void SpawnPipes()
    {
        int xSize = level.sizeX;
        int ySize = level.sizeY;

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
