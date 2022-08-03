using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public event System.Action<GameObject> onTouched;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        onTouched?.Invoke(collision.gameObject);
    }
}
