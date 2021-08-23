using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player
{
    public string Name { get; set; }
    public List<Figure> Figures { get; set; }
    public string FigureName { get; set; }

    public Player(string name = "player") { }

    public Player(List<Figure> figures, string figure, string name = "player")
    {
        Name = name;
        Figures = figures;
        FigureName = figure;
    }

    public void MoveFigure(GameObject target)
    {
        Figures.First().MoveTo(target);
    }
}
