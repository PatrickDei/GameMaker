using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance
{
    private static GameInstance instance = null;

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

    public string MapName;
    public string MovementStyle;
    public List<Player> Players;
}