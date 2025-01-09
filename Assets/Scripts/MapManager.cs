using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public Tile tileOverlayPrefab;
    public GameObject tileOverlayContainer;

    public Tile[,] map;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        var tilemaps = GetComponentsInChildren<Tilemap>();

        // Первый ребёнок должен быть тайлмапом земли, по которой будет двигаться игрок
        map = GenerateTiles(tilemaps[0]);
        if (tilemaps.Length > 1)
        {
            MakeUnreachable(tilemaps[1], map);
        }


    }

    private Tile[,] GenerateTiles(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;

        Tile[,] tiles = new Tile[bounds.max.x - bounds.min.x, bounds.max.y - bounds.min.y];

        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var tilePos = new Vector3Int(x, y);

                // На случай "дырок" в тайлмапе
                if (tilemap.HasTile(tilePos))
                {
                    var tileOverlay = Instantiate(tileOverlayPrefab, tileOverlayContainer.transform);
                    var cellWorldPos = tilemap.GetCellCenterWorld(tilePos);

                    // Вперёд на 1 по Z координате
                    tileOverlay.transform.position = cellWorldPos - Vector3.forward;

                    // Координаты тайла, где левая нижняя имеет координаты 0, 0
                    tileOverlay.gridLocation = tilePos - new Vector3Int(bounds.min.x, bounds.min.y);
                    tiles[x - bounds.min.x ,y - bounds.min.y] = tileOverlay;
                }
            }
        }

        return tiles;
    }

    public List<Tile> GetNeighbourTiles(Tile currentTile)
    {
        var map = Instance.map;

        var neghbourTiles = new List<Tile>();

        // Сверху
        Vector2Int pos = new Vector2Int(currentTile.gridLocation.x, currentTile.gridLocation.y + 1);
        if (pos.y < map.GetLength(1) && map[pos.x, pos.y] != null && map[pos.x, pos.y].isReachable)
            neghbourTiles.Add(map[pos.x, pos.y]);

        // Справа
        pos = new Vector2Int(currentTile.gridLocation.x + 1, currentTile.gridLocation.y);
        if (pos.x < map.GetLength(0) && map[pos.x, pos.y] != null && map[pos.x, pos.y].isReachable)
            neghbourTiles.Add(map[pos.x, pos.y]);

        // Снизу
        pos = new Vector2Int(currentTile.gridLocation.x, currentTile.gridLocation.y - 1);
        if (pos.y >= 0 && map[pos.x, pos.y] != null && map[pos.x, pos.y].isReachable)
            neghbourTiles.Add(map[pos.x, pos.y]);

        // Слева
        pos = new Vector2Int(currentTile.gridLocation.x - 1, currentTile.gridLocation.y);
        if (pos.x >= 0 && map[pos.x, pos.y] != null && map[pos.x, pos.y].isReachable)
            neghbourTiles.Add(map[pos.x, pos.y]);

        return neghbourTiles;
    }

    private void MakeUnreachable(Tilemap obstacles, Tile[,] tiles)
    {
        BoundsInt bounds = obstacles.cellBounds;

        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var tilePos = new Vector3Int(x, y);

                // На случай "дырок" в тайлмапе
                if (obstacles.HasTile(tilePos))
                {
                    if (tiles[x - bounds.min.x, y - bounds.min.y] != null)
                    {
                        tiles[x - bounds.min.x, y - bounds.min.y].isReachable = false;
                    }
                }
            }
        }
    }

}
