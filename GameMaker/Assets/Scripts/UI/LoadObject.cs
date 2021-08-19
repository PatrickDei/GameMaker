﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class LoadObject : MonoBehaviour
{

    [SerializeField] UnityEvent anEvent;
    [SerializeField] public string AddressablesGroup;
    private IList<GameObject> prefabs;

    public void OnEnable()
    {
        Debug.LogWarningFormat("Loading {0}", AddressablesGroup);
        Addressables.LoadAssetsAsync<GameObject>(AddressablesGroup, null).Completed += AddressablesAreReady;
    }

    public void AddressablesAreReady(AsyncOperationHandle<IList<GameObject>> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Addressables ready, invoking!");
            prefabs = handleToCheck.Result;
            anEvent.Invoke();
        }
    }

    public void LoadObjects(GameObject target)
    {
        Debug.LogFormat("Amount of addressable prefabs: {0}", prefabs.Count);

        int i = 0;
        foreach (var obj in prefabs)
        {
            GameObject newObject;

            if (gameObject.scene.name == "LevelSelect")
            {
                newObject = Instantiate(obj, new Vector3(-15f + i++ * 15f, 6.5f, 30), Quaternion.identity);
            }
            else if (obj.name == GameInstance.SharedInstance.MapName)
            {
                newObject = Instantiate(obj, new Vector3(15f, 1f, 30), Quaternion.identity);
                newObject.transform.localScale = newObject.transform.localScale * 3;
            }
            else
                continue;

            Debug.LogFormat("Parent is: {0}", target.name);
            newObject.transform.SetParent(target.transform);
            if(AddressablesGroup != "Figures")
                newObject.transform.rotation = Quaternion.AngleAxis(newObject.transform.rotation.x - 90, Vector3.right);
            newObject.name = obj.name;
        }
    }

    public void LoadChosenGameObjects(GameObject parent)
    {
        GameObject map = null;
        foreach (var obj in prefabs)
        {
            if (obj.name == GameInstance.SharedInstance.MapName)
            {
                map = Instantiate(obj, parent.transform.position, Quaternion.identity);
                //map.transform.localScale = newObject.transform.localScale * 3;

                Debug.LogFormat("Parent is: {0}", parent.name);
                map.transform.SetParent(parent.transform);
                map.transform.position = parent.transform.position;
                map.name = obj.name;
                break;
            }
        }

        GameObject figure = Instantiate(
            Resources.Load<GameObject>("prefabs/" + GameInstance.SharedInstance.Players.First().FigureName), 
            map.transform.position, 
            Quaternion.identity);
        figure.transform.parent = parent.transform;
    }
}
