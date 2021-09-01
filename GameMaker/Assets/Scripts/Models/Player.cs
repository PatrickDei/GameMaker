using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public string Name { get; set; }
    // value of pair is index of field the piece is standing on
    public List<KeyValuePair<Figure, int>> Figures { get; set; }
    public string FigureName { get; set; }

    public int Points { get; set; }

    public Player(string name = "player") {
        Points = 0;
        Name = name;
    }

    public Player(List<KeyValuePair<Figure, int>> figures, string figure, string name = "player")
    {
        Points = 0;
        Name = name;
        Figures = figures;
        FigureName = figure;
    }

    // returns index if space was already occupied
    public GameObject MoveFigure(GameObject target)
    {
        Figures.First(f => f.Key.Figurine.name == GameController.selectedFigure.name).Key.MoveTo(target);

        int selectedIndex = GameInstance.SharedInstance.Fields.First(f => f.Key == target.name).Value;
        int i = -1;
        for (i = 0; i < Figures.Count; i++)
            if (Figures[i].Key.Figurine.name == GameController.selectedFigure.name)
                break;
        Figures[i] = new KeyValuePair<Figure, int>(Figures.First(f => f.Key.Figurine.name == GameController.selectedFigure.name).Key, selectedIndex);

        if(GameInstance.SharedInstance.DestroyingFigures)
            foreach (var player in GameInstance.SharedInstance.Players)
                foreach (var figure in player.Figures)
                {
                    if (figure.Value == selectedIndex && figure.Key != Figures[i].Key)
                    {
                        Debug.LogWarning("Figure eaten");
                        player.Figures.Remove(new KeyValuePair<Figure, int>(figure.Key, figure.Value));
                        return figure.Key.Figurine;
                    }
                }
        return null;
    }

    public void UpdateStats()
    {
        GameObject player = GameObject.Find(Name);

        player.transform.GetChild(1).GetComponent<Text>().text = "Figures left " + Figures.Count;

        if(GameInstance.SharedInstance.GameEndCondition.Key == "Points scored")
            player.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Score " + Points;
    }
}
