using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class OnClickObject : MonoBehaviour
{

    [SerializeField] UnityEvent anEvent;

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
        /*if(type == "Map")
        {
            SceneController.OnSceneLoad("RuleSelection");
        }*/
    }

    public void SelectMap()
    {
        Debug.Log("Map selected");
        SelectionController.SelectItem(gameObject);

        SelectionController.NextSelectionStep("Object");
        //SwitchScene();
    }

    public void SelectFigure()
    {
        Debug.Log("Figure selected");
        SelectionController.SelectItem(gameObject);

        SceneController.OnSceneLoad("RuleSelection");
    }

    public void SelectMovementStyle()
    {
        Debug.LogFormat("Movement style: {0}", gameObject.name);
        GameInstance.SharedInstance.MovementStyle = gameObject.name;

        SelectionController.NextSelectionStep("Rule");
    }
}