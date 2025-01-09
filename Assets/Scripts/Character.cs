using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Tile standingOn;

    public float speed = 10f;

    public void FindStandingOn()
    {
        var map = MapManager.Instance.map;

        foreach (var tile in map)
        {
            if (new Vector2(tile.transform.position.x, tile.transform.position.y) == new Vector2(transform.position.x, transform.position.y))
                standingOn = tile;
        }
    }
}
