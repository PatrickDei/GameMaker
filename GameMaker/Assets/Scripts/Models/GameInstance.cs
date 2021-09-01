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
    public List<string> LethalFields = new List<string>();
    public bool DestroyingFigures = true;
    public int ScoreToWin = -1;
    public List<string> ScoringFields = new List<string>();
    public List<Star> Stars = new List<Star>();

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

            if (LethalFields.Contains(target.name)) {
                selectedPlayer.Figures.Remove(selectedPlayer.Figures.First(f => f.Key.Figurine == GameController.selectedFigure));
                Object.Destroy(GameController.selectedFigure);
            }

            if (ScoringFields.Contains(target.name))
            {
                selectedPlayer.Points++;

                Star star = Stars.First(s => s.FieldName == target.name);

                bool canPlace = true;

                foreach (var field in Fields)
                    if (!LethalFields.Contains(field.Key) && !ScoringFields.Contains(field.Key))
                    {
                        foreach (var player in Players)
                            foreach (var figure in player.Figures)
                                if (figure.Key.Figurine.name == field.Key)
                                    canPlace = false;
                        if (canPlace)
                        {
                            star.MoveTo(GameObject.Find(field.Key));
                            ScoringFields.Remove(star.FieldName);
                            star.FieldName = field.Key;
                            ScoringFields.Add(star.FieldName);
                            break;
                        }
                    }
            }
        }
        else
            Debug.LogError("Couldn't recognise figure, select it once more!");
    }
}