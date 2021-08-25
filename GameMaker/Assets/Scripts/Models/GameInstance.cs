﻿using System.Collections;
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

    public string MapName;
    public string MovementStyle;
    // value of pair marks wether it is win condition or lose condition
    public KeyValuePair<string, bool> GameEndCondition;
    public List<Player> Players;
    public List<KeyValuePair<GameObject, int>> Fields;
    private static int Turn = 0;

    public void MovePlayersFigure(GameObject target)
    {
        Object.Destroy(Players[Turn++ % Players.Count].MoveFigure(target));
    }
}