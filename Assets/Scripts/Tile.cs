using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isReachable = true;

    public int G;
    public int H;

    public int F { get { return G + H; } }

    public Vector3Int gridLocation;
    public Tile previous;
}
