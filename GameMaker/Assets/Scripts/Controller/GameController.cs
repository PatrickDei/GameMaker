using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject diceCount;

    public static void TryMoveFigure(GameObject field)
    {
        Debug.LogFormat("Selected field: {0}", field);
        if (GameInstance.SharedInstance.Fields.FirstOrDefault(f => f.Key == field.name).Key != null)
            GameInstance.SharedInstance.MovePlayersFigure(field);
        else
            Debug.LogWarning("Invalid move");
        if (!GameInstance.SharedInstance.GameEndCondition.Value && GameInstance.SharedInstance.GameEndCondition.Key == "No figures left")
            foreach (var player in GameInstance.SharedInstance.Players)
                if (player.Figures.Count == 0)
                    Debug.LogError("GameOver!");
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
            GameObject nextField = GameObject.Find(
                    GameInstance.SharedInstance.Fields[
                        (GameInstance.SharedInstance
                            .Players[GameInstance.SharedInstance.PlayerIndex]
                            .Figures[0].Value + diceValue) % GameInstance.SharedInstance.Fields.Count
                    ].Key
                );

            TryMoveFigure(nextField);
        }
    }

    void TriggerGameLost(Player loser)
    {

    }

    void TriggerGameWon(Player winner)
    {

    }
}
