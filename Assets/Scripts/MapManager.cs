using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    [SerializeField] private Tile tileOverlayPrefab;
    [SerializeField] private GameObject tileOverlayContainer;

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

        // ������ ������ ������ ���� ��������� �����, �� ������� ����� ��������� �����
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

                // �� ������ "�����" � ��������
                if (tilemap.HasTile(tilePos))
                {
                    var tileOverlay = Instantiate(tileOverlayPrefab, tileOverlayContainer.transform);
                    var cellWorldPos = tilemap.GetCellCenterWorld(tilePos);

                    // ����� �� 1 �� Z ����������
                    tileOverlay.transform.position = cellWorldPos - Vector3.forward;

                    // ���������� �����, ��� ����� ������ ����� ���������� 0, 0
                    tileOverlay.gridLocation = tilePos - new Vector3Int(bounds.min.x, bounds.min.y);
                    tiles[x - bounds.min.x ,y - bounds.min.y] = tileOverlay;
                }
            }
        }

        return tiles;
    }

    private void MakeUnreachable(Tilemap obstacles, Tile[,] tiles)
    {
        BoundsInt bounds = obstacles.cellBounds;

        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var tilePos = new Vector3Int(x, y);

                // �� ������ "�����" � ��������
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
