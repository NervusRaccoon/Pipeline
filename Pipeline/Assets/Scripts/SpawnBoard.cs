using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoard : MonoBehaviour
{
    public LevelData level;
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
        /*Transform levelTransform = level.content.transform;
        foreach(Transform child in levelTransform)
        {
            Instantiate(child.gameObject, child.position, child.rotation, pipes);
        }*/
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
        foreach(TilePos pipe in level.content)
        {
            GameObject newTile = Instantiate(pipe.tile.pref, Vector3.zero, Quaternion.identity, pipes);
            newTile.name = pipe.tile.name;
            newTile.transform.Rotate(0, 0, pipe.rotZ);
            newTile.transform.localPosition = pipe.pos;
        }            
    }

}
