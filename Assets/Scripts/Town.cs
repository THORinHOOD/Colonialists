using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour {
    public int Owner { get; private set; }
    public Coord Coord { get; private set; }

    public Town()
    {
        Owner = -1;
    }

    public void SetCoord(Coord coord)
    {
        Coord = coord;
    }

    public void SetOwner(int owner)
    {
        if (Owner == -1)
            Owner = owner;
    }
}
