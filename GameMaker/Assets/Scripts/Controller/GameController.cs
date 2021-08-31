using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject diceCount;
    public static GameObject selectedFigure = null;

    public static void TryMoveFigure(GameObject field)
    {
        Debug.LogFormat("Selected field: {0}", field);
        if (selectedFigure && GameInstance.SharedInstance.Fields.FirstOrDefault(f => f.Key == field.name).Key != null)
            GameInstance.SharedInstance.MovePlayersFigure(field);
        else {
            Debug.LogWarning("Invalid move");
            return;
        }

        if (!GameInstance.SharedInstance.GameEndCondition.Value && GameInstance.SharedInstance.GameEndCondition.Key == "No figures left")
            foreach (var player in GameInstance.SharedInstance.Players)
                if (player.Figures.Count == 0)
                    TriggerGameLost(player);

        if (GameInstance.SharedInstance.GameEndCondition.Value && GameInstance.SharedInstance.GameEndCondition.Key == "Reach goal")
            if (GameInstance.SharedInstance.WiningFields.Contains(field.name))
            {
                Player winner = null;
                foreach (Player p in GameInstance.SharedInstance.Players)
                    foreach (var f in p.Figures)
                        if (f.Key.Figurine == selectedFigure)
                            winner = p;
                TriggerGameWon(winner);
            }

    }

    public void ChangeFollowCubeAmount()
    {
        GameInstance.SharedInstance.cubeIsFollowed = !GameInstance.SharedInstance.cubeIsFollowed;
    }

    public void RollDice()
    {
        var random = new System.Random();
        int diceValue = random.Next(1, 7);
        Debug.LogFormat("Rolled {0}", diceValue);

        diceCount.GetComponent<Text>().text = diceValue.ToString();

        if (GameInstance.SharedInstance.cubeIsFollowed)
        {
            int figuresField = -1;
            foreach(Player p in GameInstance.SharedInstance.Players)
                foreach(var f in p.Figures)
                {
                    if (f.Key.Figurine == selectedFigure)
                        figuresField = f.Value;
                }
            if (figuresField != -1)
            {
                GameObject nextField = GameObject.Find(
                        GameInstance.SharedInstance.Fields[(figuresField + diceValue) 
                        % GameInstance.SharedInstance.Fields.Count]
                        .Key
                    );

                TryMoveFigure(nextField);
            }
        }
    }

    static void TriggerGameLost(Player loser)
    {
        ActivateGameOverText();
        GameObject.Find("Subtitle").GetComponent<Text>().text = loser.Name + " lost!";
    }

    static void TriggerGameWon(Player winner)
    {
        ActivateGameOverText();
        GameObject.Find("Subtitle").GetComponent<Text>().text = winner.Name + " won!";
    }

    static void ActivateGameOverText()
    {
        GameObject obj = Resources.FindObjectsOfTypeAll<GameObject>().First(n => n.name == "Game Over");
        obj.SetActive(true);
    }
}
