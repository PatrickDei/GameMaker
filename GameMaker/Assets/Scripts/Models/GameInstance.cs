using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameInstance
{
    private static GameInstance instance = null;
    private static Parameters gameParameters = null;

    public static GameInstance SharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameInstance();
            }
            return instance;
        }
    }

    public static Parameters GameParameters
    {
        get
        {
            if (gameParameters == null)
            {
                gameParameters = JsonUtility.FromJson<Parameters>(File.ReadAllText("Assets/Parameters/parameters.json"));
            }
            return gameParameters;
        }
    }

    public int PlayerIndex
    {
        get
        {
            return Turn % Players.Count;
        }
    }

    public string MapName;
    public string MovementStyle;
    // value of pair marks wether it is win condition or lose condition
    public KeyValuePair<string, bool> GameEndCondition;
    public List<Player> Players;
    // key marks the field name, value marks its position
    public List<KeyValuePair<string, int>> Fields = null;
    private static int Turn = 0;
    public bool cubeIsFollowed = false;
    public int numOfFiguresPerPlayer;
    public List<string> WiningFields = new List<string>();
    public bool DestroyingFigures = true;

    public void MovePlayersFigure(GameObject target)
    {
        Player selectedPlayer = null;
        foreach (Player p in Players)
            foreach (var figure in p.Figures)
                if (figure.Key.Figurine.name == GameController.selectedFigure.name)
                    selectedPlayer = p;

        if (selectedPlayer != null)
        {
            GameObject overlappedFigure = selectedPlayer.MoveFigure(target);
            if(DestroyingFigures)
                Object.Destroy(overlappedFigure);
        }
        else
            Debug.LogError("Couldn't recognise figure, select it once more!");
    }
}