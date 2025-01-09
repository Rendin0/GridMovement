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

    private PathFinder pathFinder;
    private RangeFinder rangeFinder;


    private List<Tile> path;
    private List<GameObject> pathObjects;
    private List<GameObject> radiusObjects;

    public CharacterSave characterSave;
    public MouseControllerSave mouseSave;

    public GameObject pathPrefab;
    public GameObject pathContainer;

    public GameObject maxLengthRadiusPrefab;
    public GameObject maxLengthRadiusContainer;

    public int maxPathLength;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Character>();
        player.transform.position = characterSave.position;
        player.FindStandingOn();

        pathFinder = new();
        path = new();
        pathObjects = new();

        rangeFinder = new();
        radiusObjects = new();


        selectedTile = MapManager.Instance.map[mouseSave.selectedPosition.x, mouseSave.selectedPosition.y];
        selectedTile.GetComponent<SpriteRenderer>().color = Color.white;
        path = pathFinder.FindPath(player.standingOn, selectedTile, maxPathLength);
        DrawMaxLengthRadius(player.standingOn, maxPathLength);
    }

    void LateUpdate()
    {
        var focusedTile = GetFocusedTile();

        if (focusedTile.collider != null)
        {
            transform.position = focusedTile.transform.position;

            // Выделение клетки и построение пути по нажатию
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

                path = pathFinder.FindPath(player.standingOn, focusedTile.collider.GetComponent<Tile>(), maxPathLength);
                DrawPath(path);
                DrawMaxLengthRadius(player.standingOn, maxPathLength);
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
                DrawPath(path);
                DrawMaxLengthRadius(player.standingOn, maxPathLength);
            }
        }
    }
    public RaycastHit2D GetFocusedTile()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return Physics2D.Raycast(mousePos, Vector2.zero);
    }

    private void DrawPath(List<Tile> Path)
    {
        foreach (var point in pathObjects)
        {
            Destroy(point);
        }
        pathObjects.Clear();


        foreach (var point in Path)
        {
            var currentPoint = Instantiate(pathPrefab, pathContainer.transform);
            pathObjects.Add(currentPoint);
            currentPoint.transform.position = point.transform.position;
        }


    }

    private void DrawMaxLengthRadius(Tile center, int radius)
    {
        List<Tile> tileRadius = rangeFinder.GetRadius(center, radius);

        foreach (var tile in radiusObjects)
        {
            Destroy(tile);
        }
        radiusObjects.Clear();

        foreach (var tile in tileRadius)
        {
            var tileObject = Instantiate(maxLengthRadiusPrefab, maxLengthRadiusContainer.transform);
            tileObject.transform.position = tile.transform.position;
            radiusObjects.Add(tileObject);
        }
    }

}
