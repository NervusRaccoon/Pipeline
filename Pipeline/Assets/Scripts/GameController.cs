using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public ProgressCheck progressCheck;
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"}; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !ProgressCheck.winDiscovered)
        {
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (ray != false)
            {
                
                GameObject tile = ray.collider.gameObject;
                if (tile.tag == tags[2] || tile.tag == tags[3])
                {
                    float prevRotation = tile.transform.eulerAngles.z; 
                    tile.transform.Rotate(0, 0, -90);
                    progressCheck.CheckWin(tile, prevRotation);
                }
            }
        }
    }
}
