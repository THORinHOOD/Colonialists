using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public int Index { get; private set; }
    public string Name { get; private set; }
    public Color Color { get; private set; }

    private List<GameObject> towns;
    private List<GameObject> roads;

    private Stock stock;

    public delegate void changingStock();
    public event changingStock changes;

	public Player(string name, int index, Color color)
    {
        Name = name;
        Color = color;
        Index = index;
        stock = new Stock();
        towns = new List<GameObject>();
    }
    
    public void addTown(GameObject town)
    {
        town.GetComponent<Town>().SetOwner(Index);
        town.GetComponentInChildren<Renderer>().material.color = Color;
        town.GetComponentInChildren<MeshRenderer>().enabled = true;
        towns.Add(town);
        Game.Log(Name + ": build a town");
    }

    public void removeRes(Game.ResourceType type, int count)
    {
        stock.addRes(type, -count);
        if (count != 0)
        {
            changes.Invoke();
            //Game.Log(Name + ": give " + count.ToString() + " " + type.ToString());
        }
    }

    public int getRes(Game.ResourceType type)
    {
        return stock.getRes(type);
    }

    public void addRes(Game.ResourceType type, int count)
    {
        stock.addRes(type, count);
        if (count != 0)
        {
            changes.Invoke();
            Game.Log(Name + ": get " + count.ToString() + " " + type.ToString());
        }
    }

    public override string ToString()
    {
        return Name + ": \n" + stock.ToString();
    }
}
