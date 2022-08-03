using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<TileData> tileList;
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"};
    public Transform board;
    private List<GameObject> startTile = new List<GameObject>();
    private Vector2[] dirRay = new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    private GameObject finish = null;
    
    private void Start()
    {
    }

    private void CheckWin()
    {
        foreach(Transform child in board)
        {
            if (child.gameObject.tag == tags[0])
                startTile.Add(child.gameObject); 
        }
        foreach(GameObject point in startTile)
        {
            Neighbours(point);
        }

        if (finish != null)
            Debug.Log("win");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (ray != false)
            {
                GameObject tile = ray.collider.gameObject;
                if (tile.tag == tags[2] || tile.tag == tags[3])
                {
                    tile.transform.Rotate(0, 0, -90);
                    CheckWin();
                }
            }
        }
    }

    private void Neighbours(GameObject startTile)
    {
        List<GameObject> list = new List<GameObject>();
        list.Add(startTile);
        GameObject prevTile = startTile;
        bool drop = false;
        while (!drop)
        {
            for (int i = 0; i < dirRay.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(prevTile.transform.position.x, prevTile.transform.position.y)
                                                    + prevTile.GetComponent<BoxCollider2D>().bounds.size.magnitude*dirRay[i], dirRay[i]); 
                if (hit.collider != null)
                {     
                    GameObject tile = hit.collider.gameObject;

                    if (prevTile.GetComponent<BoxCollider2D>().IsTouching(hit.collider) && tile != prevTile)
                    {
                        prevTile = tile;
                        list.Add(prevTile);
                        if (tile.tag == "Outgoing")
                        {
                            drop = true;
                            finish = tile;
                        }
                    }
                    else
                    {
                        drop = true;
                    }
                }
            }
        }
        Debug.Log(list.Count);
    }

    /*private void Neighbours(GameObject startTile)
    {
        GameObject prevTile = startTile;
        bool drop = false;
        int maxCount = 0;
        while (!drop)
            for (int i = 0; i < dirRay.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(prevTile.transform.position.x, prevTile.transform.position.y)
                                                    + prevTile.GetComponent<BoxCollider2D>().bounds.size.magnitude*dirRay[i], dirRay[i]); 
                if (hit.collider != null)
                {        
                    GameObject tile = hit.collider.gameObject;

                    if (tile != prevTile && (tile.tag == tags[1] || tile.tag == tags[2] || tile.tag == tags[3])) //tile != prevTile
                    {
                        string tileTag = tile.tag;

                        int angle = -1;
                        int flat = -1;
                        int prevAngle = -1;
                        int prevFlat = -1;
                        if (tileTag == tags[3])
                        {
                            angle = (int)tile.transform.rotation.z;
                            if (angle == -360) angle = 0;
                        }
                        if (tileTag == tags[2])
                        {
                            flat = (int)tile.transform.rotation.z;
                            if (flat == -180 || flat == 180) flat = 0; else if (flat == -270 || flat == 270 || flat == 90) flat = -90;
                        }
                        
                        //start //vertical
                        if (prevTile.tag == tags[0])
                        {
                            if (angle == 0 || angle == -270 || flat == -90)
                                prevTile = tile;
                        }
                        //horizontal
                        if (prevTile.tag == tags[2])
                        {
                            prevFlat = (int)prevTile.transform.rotation.z;
                            if (prevFlat == -180 || prevFlat == 180) prevFlat = 0; else if (prevFlat == -270 || prevFlat == 270 || prevFlat == -90) prevFlat = -90;

                            if (prevFlat == 0 && angle == -90)
                                prevTile = tile;
                            if (prevFlat == -90 && (angle == 0 || angle == -270 || flat == -90))
                                    prevTile = tile;
                                
                        }
                        //angle
                        if (prevTile.tag == tags[3])
                        {
                            prevAngle = (int)prevTile.transform.rotation.z;
                            if (prevAngle == -360) prevAngle = 0;

                            // 0
                            if (prevAngle == 0)
                                if (angle == -180 || flat == 0)
                                    prevTile = tile;
                            // 90 // 180
                            if (prevAngle == -90 || prevAngle == -180)
                                if (angle == -270 || angle == 0 || flat == -90)
                                    prevTile = tile;
                            // 270
                            if (prevAngle == -270)
                                if (angle == -90 || flat == 0)
                                    prevTile = tile;
                        }

                        //finish
                        if (tile.tag == tags[1])
                        {
                            if (prevFlat == -90 || prevAngle == -90 || prevAngle == -180)
                            {
                                finish = tile;
                            }
                            drop = true;
                        }

                        if (prevTile != tile)
                        {
                            drop = true;
                            Debug.Log(maxCount);
                        }
                        else
                            maxCount +=1;
                    }
                }
            }
    }*/
}
