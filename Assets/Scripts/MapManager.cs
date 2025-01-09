using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    [SerializeField] private GameObject tileOverlayPrefab;
    [SerializeField] private GameObject tileOverlayContainer;

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
        var tilemap = GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;

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
                }
            }
        }

    }

}
