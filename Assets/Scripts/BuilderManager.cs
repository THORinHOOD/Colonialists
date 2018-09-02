using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager {

    private static Stock costOfTown;
    
    static BuilderManager()
    {
        setCostOfTown();
    }

    private static void setCostOfTown()
    {
        costOfTown = new Stock();
        costOfTown.addRes(Game.ResourceType.Brick, 1);
        costOfTown.addRes(Game.ResourceType.Tree, 1);
        costOfTown.addRes(Game.ResourceType.Sheep, 1);
        costOfTown.addRes(Game.ResourceType.Wheat, 1);
    }
    
    private static bool hasEnough(Stock cost, Player player)
    {
        return (player.getRes(Game.ResourceType.Brick) >= cost.getRes(Game.ResourceType.Brick)) &&
            (player.getRes(Game.ResourceType.Rock) >= cost.getRes(Game.ResourceType.Rock)) &&
            (player.getRes(Game.ResourceType.Sheep) >= cost.getRes(Game.ResourceType.Sheep)) &&
            (player.getRes(Game.ResourceType.Tree) >= cost.getRes(Game.ResourceType.Tree)) &&
            (player.getRes(Game.ResourceType.Wheat) >= cost.getRes(Game.ResourceType.Wheat));
    }

    private static void getForBuilding(Stock cost, Player player)
    {
        player.removeRes(Game.ResourceType.Brick, cost.getRes(Game.ResourceType.Brick));
        player.removeRes(Game.ResourceType.Sheep, cost.getRes(Game.ResourceType.Sheep));
        player.removeRes(Game.ResourceType.Tree, cost.getRes(Game.ResourceType.Tree));
        player.removeRes(Game.ResourceType.Rock, cost.getRes(Game.ResourceType.Rock));
        player.removeRes(Game.ResourceType.Wheat, cost.getRes(Game.ResourceType.Wheat));
    }

    public static bool buildTown(GameObject town, Map map)
    {
        Player player = Game.currentPlayer();
        if (hasEnough(costOfTown, player))
        {
            List<GameObject> neighbors = map.townNeighbors(town.GetComponent<Town>().Coord);
            bool flag = true;
            foreach (GameObject go in neighbors)
            {
                if (go.GetComponent<Town>().Owner != -1)
                    flag = false;
            }

            if (flag)
            {
                getForBuilding(costOfTown, player);
                Game.currentPlayer().addTown(town);
                return true;
            }
        } 
        return false;
    }
}
