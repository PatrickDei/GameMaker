using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class OnClickObject : MonoBehaviour
{

    [SerializeField] public UnityEvent anEvent;

    private void OnMouseDown()
    {
        anEvent.Invoke();
    }

    public void SwitchScene()
    {
        String type = gameObject.tag;
        Debug.LogFormat("Clicked Object Of Type {0}", type);

        Action<String> callback = sceneswitch => SceneController.OnSceneLoad(gameObject.name);
        if (type == "MenuItem")
        {
            StartCoroutine(AnimationController.AnimateObject(gameObject, callback));
        }
    }

    public void SelectItem()
    {
        SelectionController.SelectItem(gameObject);
    }

    public void SelectField()
    {
        Debug.LogFormat("Field name: {0}, position x & y: {1} {2}", gameObject.name, gameObject.transform.position.x, gameObject.transform.position.y);
        GameInstance.SharedInstance.MovePlayersFigure(gameObject);
    }
}