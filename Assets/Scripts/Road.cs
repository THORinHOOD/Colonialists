using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {
    public int Owner { get; private set; }

    public Road()
    {
        Owner = -1;
    }

    public void SetOwner(int newOwner)
    {
        if (Owner == -1)
            Owner = newOwner;
    }

}
