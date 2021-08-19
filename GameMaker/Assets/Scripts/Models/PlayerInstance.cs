using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance
{
    public string Name { get; set; }
    public List<Figure> Figures { get; set; }
    public GameObject RenderableFigure { get; set; }

    public PlayerInstance(List<Figure> figures, GameObject figure, string name = "player")
    {
        Name = name;
        Figures = figures;
        RenderableFigure = figure;
    }
}
