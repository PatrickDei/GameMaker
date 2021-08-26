using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player
{
    public string Name { get; set; }
    // value of pair is index of field the piece is standing on
    public List<KeyValuePair<Figure, int>> Figures { get; set; }
    public string FigureName { get; set; }

    public Player(string name = "player") { }

    public Player(List<KeyValuePair<Figure, int>> figures, string figure, string name = "player")
    {
        Name = name;
        Figures = figures;
        FigureName = figure;
    }

    // returns index if space was already occupied
    public GameObject MoveFigure(GameObject target)
    {
        Figures.First().Key.MoveTo(target);
        Debug.LogFormat("Number of fields: {0}", GameInstance.SharedInstance.Fields.Count);
        int selectedIndex = GameInstance.SharedInstance.Fields.FirstOrDefault(f => f.Key == target.name).Value;
        Figures[0] = new KeyValuePair<Figure, int>(Figures.First().Key, selectedIndex);
        Debug.Log(selectedIndex);
        foreach (var player in GameInstance.SharedInstance.Players)
            foreach (var figure in player.Figures)
                if (figure.Value == selectedIndex && figure.Key != Figures[0].Key)
                {
                    player.Figures.Remove(new KeyValuePair<Figure, int>(figure.Key, figure.Value));
                    return figure.Key.Figurine;
                }
        return null;
    }
}
