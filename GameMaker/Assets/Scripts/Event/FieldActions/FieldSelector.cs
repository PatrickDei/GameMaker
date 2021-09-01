using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class FieldSelector : MonoBehaviour
{
    UnityEvent selectionEvent = new UnityEvent();

    public void Start()
    {
        selectionEvent.AddListener(Choose);
    }

    void OnMouseDown()
    {
        selectionEvent.Invoke();
    }

    protected abstract void Choose();
}
