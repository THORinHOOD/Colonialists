using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game {
    public enum GameState { StartGame, Game };
    static public GameState State { get; set; }
    public enum ResourceType { Tree, Rock, Brick, Sheep, Wheat, Desert };
    public static int whoPlaying { get; private set; }
    public static List<Player> players;

    private static Dice[] dices;
    
    public delegate void GiveRes(int number);
    public static event GiveRes giveResources;

    public static void StartGame()
    {
        State = GameState.StartGame;

        players = new List<Player>();
        players.Add(new Player("Jack", players.Count, Color.blue));
        players.Add(new Player("Paul", players.Count, Color.red));
        players.Add(new Player("Lana", players.Count, Color.yellow));

        for (int i = 0; i < players.Count; i++)
            players[i].changes += SetUI;
        
        whoPlaying = 0;
        
        for (int i = 0; i < players.Count; i++)
        {
            players[i].addRes(ResourceType.Brick, 2);
            players[i].addRes(ResourceType.Sheep, 2);
            players[i].addRes(ResourceType.Tree, 2);
            players[i].addRes(ResourceType.Wheat, 2);
        }
        
        dices = new Dice[2];
        dices[0] = new Dice();
        dices[1] = new Dice();

        SetUI();
    }
    
    private static void SetUI()
    {
        Text currentPlayer = GameObject.Find("Canvas/Caption_CurrentPlayer").GetComponent<Text>();
        currentPlayer.text = "Current turn : " + players[whoPlaying].Name;
        setUIResCount(ResourceType.Brick, players[whoPlaying].getRes(ResourceType.Brick));
        setUIResCount(ResourceType.Sheep, players[whoPlaying].getRes(ResourceType.Sheep));
        setUIResCount(ResourceType.Rock, players[whoPlaying].getRes(ResourceType.Rock));
        setUIResCount(ResourceType.Wheat, players[whoPlaying].getRes(ResourceType.Wheat));
        setUIResCount(ResourceType.Tree, players[whoPlaying].getRes(ResourceType.Tree));

        if (dices != null)
        {
            Text dicesInfo = GameObject.Find("Canvas/Caption_Dices").GetComponent<Text>();
            dicesInfo.text = dices[0].Number.ToString() + " + " + dices[1].Number.ToString() + " = "
                + (dices[0].Number + dices[1].Number).ToString();
        }
    }

    private static void setUIResCount(ResourceType type, int count)
    {
        Text text;
        switch(type)
        {
            case ResourceType.Brick:
                text = GameObject.Find("Canvas/Caption_Brick").GetComponent<Text>();
                text.text = "Кирпич : " + count;
                break;
            case ResourceType.Tree:
                text = GameObject.Find("Canvas/Caption_Tree").GetComponent<Text>();
                text.text = "Дерево : " + count;
                break;
            case ResourceType.Wheat:
                text = GameObject.Find("Canvas/Caption_Wheat").GetComponent<Text>();
                text.text = "Пшеница : " + count;
                break;
            case ResourceType.Rock:
                text = GameObject.Find("Canvas/Caption_Rock").GetComponent<Text>();
                text.text = "Камень : " + count;
                break;
            case ResourceType.Sheep:
                text = GameObject.Find("Canvas/Caption_Sheep").GetComponent<Text>();
                text.text = "Овца : " + count;
                break;
            default:
                break;
        }
    }
    

    public static Player currentPlayer()
    {
        return players[whoPlaying];
    }

    public static void endTurn()
    {
        Log(currentPlayer().Name + ": end of turn");
        whoPlaying = (whoPlaying + 1) % players.Count;
        Log(currentPlayer().Name + ": start of turn");
        RollAllDices();
        SetUI();
    }
    
    private static void RollAllDices()
    {
        int sum = 0;
        if (dices != null)
            for (int i = 0; i < dices.Length; i++)
                if (dices[i] != null)
                {
                    dices[i].Roll();
                    sum += dices[i].Number;
                }
        giveResources.Invoke(sum);
    }
    
    private class Dice
    {
        public int Number { get; private set; }
        public Dice()
        {
            Number = Random.Range(1, 7);
        }

        public void Roll()
        {
            Number = Random.Range(1, 7);
        }
    }
    
    public static void Log(string message)
    {
        GameObject.Find("ConsoleView").GetComponent<ConsoleView>().Log(message + "\n");
    }

    public static ResourceType getResByName(string name)
    {
        name = name.ToLower();
        switch (name)
        {
            case "rock":
                return ResourceType.Rock;
            case "sheep":
                return ResourceType.Sheep;
            case "tree":
                return ResourceType.Tree;
            case "wheat":
                return ResourceType.Wheat;
            case "brick":
                return ResourceType.Brick;
            default:
                return ResourceType.Desert;
        }
    }

    public static int getPlayerByName(string name)
    {
        int player = -1;
        for (int i = 0; i < players.Count; i++)
            if (players[i].Name.Equals(name))
                player = i;
        return player;
    }
}
