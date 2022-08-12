using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlController : MonoBehaviour
{
    private GameController gameController;
    private List<string> tags = new List<string>{"Incoming", "Outgoing", "Flat", "Angle"}; 

    private void Start()
    {
        gameController = this.GetComponent<GameController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameController.winDiscovered)
        {
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (ray != false)
            {
                GameObject cell = ray.collider.gameObject;
                if (cell.tag == tags[2] || cell.tag == tags[3])
                    gameController.RotateCell(cell);
            }
        }
    }
}
