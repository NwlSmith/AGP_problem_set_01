using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class SMITH_PS_W5 : MonoBehaviour
{

    /*
     * Write a CSV parser - that takes in a CSV file of players and returns a list of those players as class objects.
     * To help you out, I've defined the player class below.  An example save file is in the folder "CSV Examples".
     *
     * NOTES:
     *     - the first row of the file has the column headings: don't include those!
     *     - location is tricky - because the location has a comma in it!!
     */

    private readonly char _lineSeparator = '\n';
    private readonly char _valueSeparator = ',';

    private class Player
    {
        public enum Class : byte
        {
            Undefined = 0,
            Monk,
            Wizard,
            Druid,
            Thief,
            Sorcerer
        }
        
        public Class classType;
        public string name;
        public uint maxHealth;
        public int[] stats;
        public bool alive;
        public Vector2 location;
    }

    private Player ParseLine(string line)
    {
        Player player = new Player();
        
        string[] elements = line.Split(_valueSeparator);

        player.name = elements[0];


        switch (elements[1])
        {
            case "Monk":
                player.classType = Player.Class.Monk;
                break;
            case "Wizard":
                player.classType = Player.Class.Wizard;
                break;
            case "Druid":
                player.classType = Player.Class.Druid;
                break;
            case "Thief":
                player.classType = Player.Class.Thief;
                break;
            case "Sorcerer":
                player.classType = Player.Class.Sorcerer;
                break;
            case "0":
                player.classType = Player.Class.Undefined;
                break;
        }

        player.maxHealth = uint.Parse(elements[2]);

        int[] stats = new int[5];
        stats[0] = int.Parse(elements[3]);
        stats[1] = int.Parse(elements[4]);
        stats[2] = int.Parse(elements[5]);
        stats[3] = int.Parse(elements[6]);
        stats[4] = int.Parse(elements[7]);
        player.stats = stats;

        player.alive = bool.Parse(elements[8]);

        Vector2 loc = new Vector2(float.Parse(elements[9].Substring(1)), float.Parse(elements[10].Substring(0, elements[10].Length - 2)));

        player.location = loc;

        return player;
    }

    private List<Player> CSVParser(TextAsset toParse)
    {
        var toReturn = new List<Player>();

        string[] lines = toParse.text.Split(_lineSeparator);

        // skip first line.
        for (int i = 1; i < lines.Length-1; i++)
        {
            Player player = ParseLine(lines[i]);
            toReturn.Add(player);
        }

        return toReturn;
    }

    /*
     * Provided is a high score list as a JSON file.  Create the functions that will find the highest scoring name, and
     * the number of people with a score above a score.
     *
     * I've included a library "SimpleJSON", which parses JSON into a dictionary of strings to strings or dictionaries
     * of strings.
     *
     * JSON.Parse(someJSONString)["someKey"] will return either a string value, or a Dictionary of strings to
     * JSON objects.
     */

    private struct ScoreTuple
    {
        public string name;
        public uint score;

        public ScoreTuple(string n, uint s)
        {
            name = n;
            score = s;
        }
    }
    public int NumberAboveScore(TextAsset jsonFile, int score)
    {
        var toReturn = 0;
        JSONNode highScores = JSON.Parse(jsonFile.text)["highScores"];
        
        foreach (JSONNode node in highScores)
        {
            if (node["score"].AsInt > score) toReturn++;
        }

        return toReturn;
    }

    public string GetHighScoreName(TextAsset jsonFile)
    {
        JSONNode highScores = JSON.Parse(jsonFile.text)["highScores"];

        ScoreTuple highest = new ScoreTuple("", 0);

        foreach (JSONNode node in highScores)
        {
            uint score = (uint)node["score"].AsInt;
            if (score > highest.score)
                highest = new ScoreTuple(node["player"], score);
        }

        return highest.name;
    }
    
    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TMPro.TextMeshProUGUI csvTest, networkTest;
    public TextAsset csvExample, jsonExample;
    private Coroutine checkingScores;
    
    private void Update()
    {
        csvTest.text = "CSV Parser\n<align=left>\n";

        var parsedPlayers1 = CSVParser(csvExample);

        if (parsedPlayers1.Count == 0)
        {
            csvTest.text += "Need to return some players.";
        }
        else
        {
            csvTest.text += Success(parsedPlayers1.Any(p => p.name == "Jeff") &&
                                    parsedPlayers1.Any(p => p.name == "Stonks")
                            ) + " read in player names correctly.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Jeff").alive &&
                        !parsedPlayers1.First(p => p.name == "Stonks").alive) + " Correctly read 'alive'.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Stonks").classType == Player.Class.Wizard &&
                        parsedPlayers1.First(p => p.name == "Twergle").classType == Player.Class.Thief) +
                " Correctly read player class.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Fortune").location == new Vector2(12.322f, 42f)) +
                " Correctly read in location.\n";
            csvTest.text += Success(parsedPlayers1.First(p => p.name == "Jeff").maxHealth == 23) +
                            " Correctly read in max health.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Fortune").location == new Vector2(12.322f, 42f)) +
                " Correctly read in location.\n";
        }
        
        networkTest.text = "JSON Data\n<align=left>\n";
        networkTest.text += Success(NumberAboveScore(jsonExample, 10) == 6) + " number above score worked correctly.\n";
        networkTest.text += Success(GetHighScoreName(jsonExample) == "GUW") + " get high score name worked correctly.\n";
    }
    
    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
}