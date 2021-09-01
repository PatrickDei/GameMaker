using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalFieldSelector : FieldSelector
{
    protected override void Choose()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        GameInstance.SharedInstance.WiningFields.Add(gameObject.name); 
        Debug.LogFormat("Field {0} selected as field {1}", gameObject.name, GameInstance.SharedInstance.Fields.Count);
    }
}
