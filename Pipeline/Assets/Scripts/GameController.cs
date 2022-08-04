using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //public List<TileData> tileList;
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"};
    public Transform board;
    private List<GameObject> startTile = new List<GameObject>();
    private Vector2[] dirRay = new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    private GameObject finish = null;
    
    private void Start()
    {
        foreach(Transform child in board)
        {
            if (child.gameObject.tag == tags[0])
                startTile.Add(child.gameObject); 
        }
    }

    private void CheckWin()
    {
        foreach(GameObject point in startTile)
        {
            Neighbours(point, point, false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && finish == null)
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

    private void Neighbours(GameObject curr, GameObject prev, bool stop)
    {
        GameObject prevTile = prev;
        GameObject tile = curr;
        List<Collision2D> list = new List<Collision2D>();
        foreach(Collision2D col in tile.GetComponent<CollisionCheck>().collisions)
            list.Add(col);

        if (list.Count == 0)
            stop = true;
        else if (list.Count == 1)
        {
            if (list[0].gameObject.tag == tags[1])
            {
                stop = true;
                finish = list[0].gameObject;
            }
            if (tile.tag == tags[0])
            {
                tile = list[0].gameObject;
                stop = true;
                Neighbours(tile, prevTile, false); 
            }
        }
        else if (list.Count == 2)
        {
            bool touchPrev = false;
            bool touchNext = false; 

        }
        if (!stop)
        {
            bool touchPrev = false;
            bool touchNext = false;
            
            foreach(Collision2D col in list)
            {
                GameObject go = col.gameObject;
                Debug.Log(go.name);
                if (go == prevTile)
                {
                    Debug.Log("touchPrev");
                    touchPrev = true;
                }
                if (go != tile)
                {
                    Debug.Log("touchNext");
                    prevTile = tile;
                    tile = go;
                    Neighbours(tile, prevTile, stop); 
                    touchNext = true;
                }
            } 
            if (touchPrev && touchNext)
            {
                //Debug.Log(tile.name);
                Debug.Log("rec");
                Neighbours(tile, prevTile, stop); 
            }             
        }
    }
}
