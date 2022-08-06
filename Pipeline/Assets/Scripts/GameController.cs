using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static System.Action<GameObject, float> rotateTube;
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"};

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !ProgressCheck.finish)
        {
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (ray != false)
            {
                GameObject tile = ray.collider.gameObject;
                if (tile.tag == tags[2] || tile.tag == tags[3])
                {
                    float prevRotation = tile.transform.eulerAngles.z; 
                    tile.transform.Rotate(0, 0, -90);
                    rotateTube?.Invoke(tile, prevRotation);
                }
            }
        }
    }
}
