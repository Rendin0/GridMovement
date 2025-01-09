using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder
{
    public List<Tile> GetRadius(Tile center, int maxRadius)
    {
        List<Tile> radius = new();
        int stepCount = 0;

        radius.Add(center);

        List<Tile> tilesForPreviousStep = new();
        tilesForPreviousStep.Add(center);

        while (stepCount < maxRadius)
        {
            List<Tile> neighbourTiles = new();

            foreach (var tile in tilesForPreviousStep)
            {
                neighbourTiles.AddRange(MapManager.Instance.GetNeighbourTiles(tile));
            }

            radius.AddRange(neighbourTiles);
            tilesForPreviousStep = neighbourTiles.Distinct().ToList();
            stepCount++;
        }

        return radius.Distinct().ToList();
    }
}
