using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseContorller : MonoBehaviour
{

    private GameObject selectedTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTile = GetFocusedTile();

        if (focusedTile.collider != null)
        {
            transform.position = focusedTile.transform.position;

            if (Input.GetMouseButtonDown(0))
            {
                if (selectedTile != null)
                {
                    // Делаем объект невидимым
                    SpriteRenderer selectedSprite = selectedTile.GetComponent<SpriteRenderer>();
                    selectedSprite.color = selectedSprite.color * new Color(1, 1, 1, 0);
                }

                SpriteRenderer focusedSprite = focusedTile.collider.gameObject.GetComponent<SpriteRenderer>();

                // Делаем объект видимым
                focusedSprite.color = focusedSprite.color + Color.black;
                selectedTile = focusedSprite.gameObject;
            }
        }
    }

    public RaycastHit2D GetFocusedTile()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return Physics2D.Raycast(mousePos, Vector2.zero);
    }
}
