using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinder
{
    public List<Tile> FindPath(Tile start, Tile end, int maxLength)
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
                return GetFinishedList(start, end, maxLength);
            }

            var neighbourTiles = MapManager.Instance.GetNeighbourTiles(currentTile);
            
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

    private List<Tile> GetFinishedList(Tile start, Tile end, int maxLength)
    {
        var finishedList = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }
        finishedList.Reverse();

        if (finishedList.Count >= maxLength)
        {
            finishedList.RemoveRange(maxLength, finishedList.Count - maxLength);
        }

        return finishedList;

    }

    private int GetManhattenDistance(Tile start, Tile neighbourTile)
    {
        return Mathf.Abs(start.gridLocation.x - neighbourTile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbourTile.gridLocation.y);
    }


}
