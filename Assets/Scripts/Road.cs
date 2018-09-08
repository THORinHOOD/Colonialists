using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {
    public int Owner { get; private set; }
    public Coord From { get; private set; }
    public Coord To { get; private set; }

    public Road()
    {
        Owner = -1;
        From = null;
        To = null;
    }

    public void SetOwner(int newOwner)
    {
        if (Owner == -1)
            Owner = newOwner;
    }

    public void SetFrom(Coord from)
    {
        if (From == null)
            From = from;
    }

    public void SetTo(Coord to)
    {
        if (To == null)
            To = to;
    }
}
