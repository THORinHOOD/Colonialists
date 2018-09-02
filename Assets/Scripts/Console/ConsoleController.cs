using UnityEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

public class ConsoleController
{
    private List<string> logList;
    public string[] log { get; private set; }
    
    #region Event declarations
    public delegate void LogChangedHandler(string[] log);
    public event LogChangedHandler logChanged;

    public delegate void VisibilityChangedHandler(bool visible);
    public event VisibilityChangedHandler visibilityChanged;
    #endregion
    
    private Dictionary<string, Command> commands;

    public ConsoleController()
    {
        logList = new List<string>();
        commands = new Dictionary<string, Command>();
        registerCommand("!echo", echo, "Just print args");
        registerCommand("!help", help, "Print this help.");
        registerCommand("!clear", clear, "Clear console");
        registerCommand("!stock", stock, "Get information about stock of a player");
        registerCommand("!give", give, "Give res to player <player> <resource> <count>");
        registerCommand("!change", change, "Change res between players <player1> <resource1> <count1> <player2> <resource2> <count2>");
    }

    private void registerCommand(string command, CommandHandler handler, string help)
    {
        commands.Add(command, new Command(command, handler, help));
    }
    
    public void runCommandString(string commandString)
    {
        appendLogLine(commandString);
        string[] args = commandString.Split(' ');
        string[] realArgs = new string[args.Length - 1];
        for (int i = 1; i < args.Length; i++)
            realArgs[i - 1] = args[i];
        runCommand(args[0], realArgs);
	}

    public void appendLogLine(string line)
    {
        string[] lines = line.Split('\n');
        while (logList.Count + lines.Length > 100)
            logList.RemoveAt(0);
        logList.AddRange(lines);
        if (logChanged != null)
            logChanged(logList.ToArray());
    }

    public void runCommand(string command, string[] args)
    {
        Command cmnd = null;
        if (!commands.TryGetValue(command, out cmnd))
        {
            if (!command.Equals(""))
                appendLogLine(string.Format("Unknown command '{0}', type '!help' for list.\n", command));
        }
        else
        {
            if (cmnd.handler == null)
                appendLogLine(string.Format("Unable to process command '{0}', handler was null.\n", command));
            else
                cmnd.handler(args);
        }
    }

    public delegate void CommandHandler(string[] args);
    private class Command
    {
        public string command { get; private set; }
        public CommandHandler handler { get; private set; }
        public string help { get; private set; }

        public Command(string command, CommandHandler handler, string help)
        {
            this.command = command;
            this.handler = handler;
            this.help = help;
        }
    }

    #region Command handlers
    private void change(string[] args)
    {
        if (args.Length < 6)
        {
            appendLogLine("Error, for !stock is necessary 6 arguments at least\n");
            return;
        }

        int player1 = Game.getPlayerByName(args[0]);
        if (player1 == -1)
        {
            appendLogLine("Error, " + args[0] + " not found player with this name\n");
            return;
        }
        Game.ResourceType res1 = Game.getResByName(args[1]);
        if (res1 == Game.ResourceType.Desert)
        {
            appendLogLine("Error, " + args[1] + " not found res with this name\n");
            return;
        }
        int count1;
        if (!Int32.TryParse(args[2], out count1))
        {
            appendLogLine("Error, " + args[2] + " is not number\n");
            return;
        }
        
        int player2 = Game.getPlayerByName(args[3]);
        if (player2 == -1)
        {
            appendLogLine("Error, " + args[3] + " not found player with this name\n");
            return;
        }
        Game.ResourceType res2 = Game.getResByName(args[4]);
        if (res2 == Game.ResourceType.Desert)
        {
            appendLogLine("Error, " + args[1] + " not found res with this name\n");
            return;
        }
        int count2;
        if (!Int32.TryParse(args[5], out count2))
        {
            appendLogLine("Error, " + args[2] + " is not number\n");
            return;
        }
        
        Game.players[player1].addRes(res2, count2);
        Game.players[player2].removeRes(res2, count2);
        Game.players[player1].removeRes(res1, count1);
        Game.players[player2].addRes(res1, count1);
    }

    private void echo(string[] args)
    {
        string lines = "";
        foreach (string arg in args)
            lines += arg + " ";
        lines += "\n";
        appendLogLine(lines);
    }

    private void help(string[] args)
    {
        string line = "Example : !<command name> <args>\n";
        foreach (string cmnd in commands.Keys)
            line += cmnd + " - " + commands[cmnd].help + "\n";
        appendLogLine(line);
    }

    private void clear(string[] args)
    {
        logList.Clear();
        logChanged(logList.ToArray());
    }

    private void give(string[] args)
    {
        if (args.Length < 3)
        {
            appendLogLine("Error, for !stock is necessary 3 arguments at least\n");
            return;
        }

        string player = args[0];
        string res = args[1];
        string count = args[2];

        int toFind_player = Game.getPlayerByName(player);

        if (toFind_player == -1)
        {
            appendLogLine("Error, " + player + " not found player with this name\n");
            return;
        }

        Game.ResourceType toFind_res = Game.getResByName(res);
        if (toFind_res == Game.ResourceType.Desert)
        {
            appendLogLine("Error, " + res + " not found res with this name\n");
            return;
        }

        int toFind_count;
        if (Int32.TryParse(count, out toFind_count))
            Game.players[toFind_player].addRes(toFind_res, toFind_count);
        else
        {
            appendLogLine("Error, " + count + " is not number\n");
            return;
        }
    }

    private void stock(string[] args)
    {
        if (args.Length < 1)
        {
            appendLogLine("Error, for !stock is necessary one argument at least\n");
            return;
        }

        string player = args[0];
        int toFind = Game.getPlayerByName(player);
         
        if (toFind == -1)
        {
            appendLogLine("Error, " + player + " not found player with this name\n");
            return;
        }
        appendLogLine(Game.players[toFind].ToString());
    }
    #endregion

}