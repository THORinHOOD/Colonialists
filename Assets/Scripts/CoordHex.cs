using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coord
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Coord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return X + "_" + Y;
    }

    public override bool Equals(object obj)
    {
        Coord other = (Coord) obj;
        return other.X == X && other.Y == Y;
    }
}
