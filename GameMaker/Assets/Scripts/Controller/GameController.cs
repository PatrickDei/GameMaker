using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController
{

    public static void TryMoveFigure(GameObject field)
    {
        Debug.LogFormat("Selected field: {0}", field);
        if (GameInstance.SharedInstance.Fields.FirstOrDefault(f => f.Key == field.name).Key != null)
            GameInstance.SharedInstance.MovePlayersFigure(field);
        else
            Debug.LogWarning("Invalid move");
        if (!GameInstance.SharedInstance.GameEndCondition.Value && GameInstance.SharedInstance.GameEndCondition.Key == "Amount of figures left")
            foreach (var player in GameInstance.SharedInstance.Players)
                if (player.Figures.Count == 0)
                    Debug.LogError("GameOver!");
    }
}
