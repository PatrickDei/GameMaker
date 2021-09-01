using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathFieldSelector : FieldSelector
{
    protected override void Choose()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        GameInstance.SharedInstance.LethalFields.Add(gameObject.name);
        Debug.LogFormat("Field {0} selected as lethal field", gameObject.name);
    }
}
