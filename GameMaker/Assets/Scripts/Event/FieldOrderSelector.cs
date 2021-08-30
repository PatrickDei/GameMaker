using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldOrderSelector : MonoBehaviour
{
    UnityEvent selectionEvent = new UnityEvent();

    public void Start()
    {
        selectionEvent.AddListener(ChooseAsNext);
    }

    void OnMouseDown()
    {
        selectionEvent.Invoke();
    }

    void ChooseAsNext()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        GameInstance.SharedInstance.Fields.Add(new KeyValuePair<string, int>(gameObject.name, GameInstance.SharedInstance.Fields.Count)); 
        Debug.LogFormat("Field {0} selected as field {1}", gameObject.name, GameInstance.SharedInstance.Fields.Count);
    }
}
