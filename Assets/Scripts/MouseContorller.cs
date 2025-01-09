using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseContorller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var focusedTile = GetFocusedTile();

        if (focusedTile.collider != null)
        {
            transform.position = focusedTile.transform.position;
        }
    }

    public RaycastHit2D GetFocusedTile()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return Physics2D.Raycast(mousePos, Vector2.zero);
    }
}
