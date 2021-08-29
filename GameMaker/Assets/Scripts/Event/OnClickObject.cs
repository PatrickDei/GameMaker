using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEditor;

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
        GameController.TryMoveFigure(gameObject);
    }

    public void FinishMap()
    {
        GameObject map = GameObject.Find("NewMap");
        Debug.LogFormat("Saving map {0}!", map.name);
        map.AddComponent<OnClickObject>();
        map.AddComponent<BoxCollider>().size = new Vector3(7f, 1.3f, 7f);
        map.tag = "Map";

        foreach (Transform child in map.transform.GetChild(0).transform)
            child.position = new Vector3(child.position.x, 0, child.position.z);

        // Set the path as within the Assets folder,
        // and name it as the GameObject's name with the .Prefab format
        string localPath = "Assets/Resources/Prefabs/Created Maps/" + map.name + ".prefab";

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab.
        GameObject createdObject = PrefabUtility.SaveAsPrefabAssetAndConnect(map, localPath, InteractionMode.UserAction);
        Debug.Log(createdObject.name);
        SceneController.OnSceneLoad("MainMenu");
    }

    public void AddMapClickListener()
    {
        anEvent = new UnityEvent();
        anEvent.AddListener(SelectItem);
    }

    public void AddFieldClickListener()
    {
        anEvent = new UnityEvent();
        anEvent.AddListener(SelectField);
    }
}