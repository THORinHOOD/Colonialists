using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock  {
    public int Sheep;
    public int Wheat;
    public int Rock;
    public int Brick;
    public int Tree;
    
    public Stock()
    {
        Sheep = 0;
        Wheat = 0;
        Rock = 0;
        Brick = 0;
        Tree = 0;
    }

    public void addRes(Game.ResourceType type, int count)
    {
        switch (type)
        {
            case Game.ResourceType.Brick:
                Brick += count;
                break;
            case Game.ResourceType.Sheep:
                Sheep += count;
                break;
            case Game.ResourceType.Wheat:
                Wheat += count;
                break;
            case Game.ResourceType.Rock:
                Rock += count;
                break;
            case Game.ResourceType.Tree:
                Tree += count;
                break;
            default:
                break;
        }
    }

    public int getRes(Game.ResourceType type)
    {
        switch(type)
        {
            case Game.ResourceType.Brick:
                return Brick;
            case Game.ResourceType.Sheep:
                return Sheep;
            case Game.ResourceType.Wheat:
                return Wheat;
            case Game.ResourceType.Rock:
                return Rock;
            case Game.ResourceType.Tree:
                return Tree;
            default:
                return -1;
        }
    }

    public override string ToString()
    {
        string toReturn =
        "Камень : " + Rock + "\n" +
        "Дерево : " + Tree + "\n" +
        "Кирпич : " + Brick + "\n" +
        "Овца : " + Sheep + "\n" +
        "Пшеница : " + Wheat + "\n";
        return toReturn;
    }
}
