using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager {

    private static Stock costOfTown;
    private static Stock costOfRoad;
    
    static BuilderManager()
    {
        setCostOfTown();
        setCostOfRoad();
    }

    private static void setCostOfRoad()
    {
        costOfRoad = new Stock();
        costOfRoad.addRes(Game.ResourceType.Brick, 1);
        costOfRoad.addRes(Game.ResourceType.Tree, 1);
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

    private static bool possibleToBuildRoad(Road road, Player player, Map map)
    {
        GameObject from = map.getTown(road.From.ToString());
        GameObject to = map.getTown(road.To.ToString());

        bool hasTownFrom = from != null && from.GetComponent<Town>().Owner == player.Index;
        bool hasTownTo = to != null && to.GetComponent<Town>().Owner == player.Index;
        bool hasTownNear = hasTownFrom || hasTownTo;

        List<GameObject> neighboursFrom = map.townNeighbors(from.GetComponent<Town>().Coord);
        List<GameObject> neighboursTo = map.townNeighbors(to.GetComponent<Town>().Coord);

        bool hasRoadFrom = false;
        bool hasRoadTo = false;

        //FROM
        foreach (GameObject near in neighboursFrom)
        {
            GameObject currentRoad = map.getRoad(near.GetComponent<Town>().Coord.ToString() + "-"
                                                 + from.GetComponent<Town>().Coord.ToString());
            if (currentRoad.GetComponent<Road>().Owner == player.Index)
                hasRoadFrom = true;
        }

        //TO
        foreach (GameObject near in neighboursTo)
        {
            GameObject currentRoad = map.getRoad(near.GetComponent<Town>().Coord.ToString() + "-"
                                                 + to.GetComponent<Town>().Coord.ToString());
            if (currentRoad.GetComponent<Road>().Owner == player.Index)
                hasRoadFrom = true;
        }

        bool hasRoad = hasRoadFrom || hasRoadTo;

        return hasTownNear || hasRoad;
    }
    
    public static bool buildRoad(GameObject road, Map map)
    {
        
        Player player = Game.currentPlayer();
        if (hasEnough(costOfRoad, player))
        {
            if (possibleToBuildRoad(road.GetComponent<Road>(), player, map))
            {
                getForBuilding(costOfRoad, player);
                Game.currentPlayer().addRoad(road);
                return true;
            } 
        }
        return false;
    }

    //TODO
    private static bool possibleToBuildTown(Town town, Player player, Map map)
    {
        List<GameObject> neighbors = map.townNeighbors(town.GetComponent<Town>().Coord);
        bool flag = true;
        foreach (GameObject go in neighbors)
        {
            if (go.GetComponent<Town>().Owner != -1)
                flag = false;
        }

        bool hasNeighbors = !flag;

        int countHisRoads = 0;
        int countOtherRoads = 0;

        foreach (GameObject near in neighbors)
        {
            GameObject buf = map.getRoad(near.GetComponent<Town>().Coord.ToString() + "-" + town.Coord.ToString());
            Road currentRoad = buf.GetComponent<Road>();
            if (currentRoad.Owner == player.Index)
                countHisRoads++;
            else if (currentRoad.Owner != -1)
                countOtherRoads++;
        }
        
        bool normWithRoads = (countHisRoads >= 1) && (countOtherRoads <= 1);

        if (Game.State == Game.GameState.StartGame)
        {
            return !hasNeighbors;
        } else
        {
            return !hasNeighbors && normWithRoads;
        }
    }

    public static bool buildTown(GameObject town, Map map)
    {
        Player player = Game.currentPlayer();
        if (hasEnough(costOfTown, player))
        {
            if (possibleToBuildTown(town.GetComponent<Town>(), player, map))
            {
                getForBuilding(costOfTown, player);
                Game.currentPlayer().addTown(town);
                return true;
            }
        } 
        return false;
    }
}
