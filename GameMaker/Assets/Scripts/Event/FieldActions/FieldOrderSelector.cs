using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldOrderSelector : FieldSelector
{
    protected override void Choose()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        GameInstance.SharedInstance.Fields.Add(new KeyValuePair<string, int>(gameObject.name, GameInstance.SharedInstance.Fields.Count)); 
        Debug.LogFormat("Field {0} selected as field {1}", gameObject.name, GameInstance.SharedInstance.Fields.Count);
    }
}
