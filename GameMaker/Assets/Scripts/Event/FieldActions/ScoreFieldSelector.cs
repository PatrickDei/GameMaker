using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreFieldSelector : FieldSelector
{
    protected override void Choose()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        GameInstance.SharedInstance.ScoringFields.Add(gameObject.name);
        Debug.LogFormat("Field {0} selected as scoring field", gameObject.name);
    }
}
