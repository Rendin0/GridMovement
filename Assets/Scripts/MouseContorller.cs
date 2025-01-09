using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseContorller : MonoBehaviour
{

    private GameObject selectedTile;

    private Character player;

    private PathFinder finder;

    private List<Tile> path;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Character>();
        finder = new();
        path = new();
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

                if (player.standingOn == null)
                    player.FindStandingOn();

                path = finder.FindPath(player.standingOn, focusedTile.collider.GetComponent<Tile>());
            }
        }
    }

    private void Update()
    {


        if (path.Count > 0)
        {
            float z = path[0].transform.position.z;
            player.transform.position = Vector2.MoveTowards(player.transform.position, path[0].transform.position, player.speed * Time.deltaTime);
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, z);
            player.standingOn = path[0];

            if (Vector2.Distance(player.transform.position, path[0].transform.position) <= 0.01f)
            {
                path.RemoveAt(0);
            }
        }
    }

    public RaycastHit2D GetFocusedTile()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return Physics2D.Raycast(mousePos, Vector2.zero);
    }
}
