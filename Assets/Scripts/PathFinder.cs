using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinder
{
    public List<Tile> FindPath(Tile start, Tile end)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            Tile currentTile = openList.OrderBy(x => x.F).First();
            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == end)
            {
                return GetFinishedList(start, end);
            }

            var neighbourTiles = GetNeighbourTiles(currentTile);
            
            foreach(var neighbourTile in neighbourTiles)
            {
                if (!closedList.Contains(neighbourTile))
                {
                    neighbourTile.G = GetManhattenDistance(start, neighbourTile);
                    neighbourTile.H = GetManhattenDistance(end, neighbourTile);
                    neighbourTile.previous = currentTile;

                    if (!openList.Contains(neighbourTile))
                    {
                        openList.Add(neighbourTile);
                    }

                }
            }

        }

        return new List<Tile>();
    }

    private List<Tile> GetFinishedList(Tile start, Tile end)
    {
        var finishedList = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }
        finishedList.Reverse();

        return finishedList;

    }

    private int GetManhattenDistance(Tile start, Tile neighbourTile)
    {
        return Mathf.Abs(start.gridLocation.x - neighbourTile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbourTile.gridLocation.y);
    }

    private List<Tile> GetNeighbourTiles(Tile currentTile)
    {
        var map = MapManager.Instance.map;

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

}
