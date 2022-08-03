using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoard : MonoBehaviour
{
    public LevelData level;
    private Transform levelTransform;
    private Transform board;
    private Quaternion[] dir = new Quaternion[] {Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, -180), Quaternion.Euler(0, 0, -270)};
    private const string flatTag = "Flat";
    private const string angleTag = "Angle";

    private void Start()
    {
        board = this.transform;
        Transform levelTransform = level.content.transform;
        foreach(Transform child in levelTransform)
        {
            Instantiate(child.gameObject, child.position, Quaternion.identity, board);
        }
        Randomize();
    }

    private void Randomize()
    {
        foreach(Transform child in board)
            if (child.gameObject.tag == angleTag || child.gameObject.tag == flatTag)
                child.rotation = dir[Random.Range(0, dir.Length-1)];
    }

    /*private void Start()
    {
        int xSize = size;
        int ySize = size;
        GameObject[,] tileArr = new GameObject[xSize, ySize];
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = tileGO.GetComponent<SpriteRenderer>().bounds.size;

        int count = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(tileGO, transform.position, Quaternion.identity);
                newTile.transform.position = new Vector3(xPos + (tileSize.x*x), yPos + (tileSize.y*y), 0);
                newTile.transform.parent = transform;
                newTile.name = count.ToString();
                count++;

                tileArr[x, y] = newTile;
            }            
        }
    }*/

}
