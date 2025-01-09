using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Save/Mouse")]

public class MouseControllerSave : ScriptableObject
{
    public Vector3Int selectedPosition;
}

public class MouseContorller : MonoBehaviour
{

    private Tile selectedTile;

    private Character player;

    private PathFinder finder;

    private List<Tile> path;

    public CharacterSave characterSave;
    public MouseControllerSave mouseSave;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Character>();
        player.transform.position = characterSave.position;
        player.FindStandingOn();
        finder = new();
        path = new();

        selectedTile = MapManager.Instance.map[mouseSave.selectedPosition.x, mouseSave.selectedPosition.y];
        selectedTile.GetComponent<SpriteRenderer>().color = Color.white;
        path = finder.FindPath(player.standingOn, selectedTile);
    }

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
                selectedTile = focusedSprite.GetComponent<Tile>();
                mouseSave.selectedPosition = selectedTile.gridLocation;

                path = finder.FindPath(player.standingOn, focusedTile.collider.GetComponent<Tile>());
            }
        }
    }

    // Update is called once per frame
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
                characterSave.position = path[0].transform.position;
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
